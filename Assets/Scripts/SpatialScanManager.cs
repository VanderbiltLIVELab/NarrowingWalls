
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpatialScanManager : Singleton<SpatialScanManager>, ISourceStateHandler, IInputClickHandler
{
    private string spaceQueryDescription;
    private string objectPlacementDescription;
    private uint trackedHandsCount = 0;

    [Tooltip("Minimum area to be scanned before status appears")]
    public float kMinAreaForStats = 5.0f;
    [Tooltip("Minimum area to be scanned before scanning could be completed")]
    public float kMinAreaForComplete = 50.0f;
    [Tooltip("Minimum horizontal area to be scanned before scanning could be completed")]
    public float kMinHorizAreaForComplete = 25.0f;
    [Tooltip("Minimum wall area to be scanned before scanning could be completed")]
    public float kMinWallAreaForComplete = 10.0f;

    public TextMesh DebugDisplay;
    public TextMesh DebugSubDisplay;

    public string SpaceQueryDescription
    {
        get
        {
            return spaceQueryDescription;
        }
        set
        {
            spaceQueryDescription = value;
            objectPlacementDescription = "";
        }
    }

    public string ObjectPlacementDescription
    {
        get
        {
            return objectPlacementDescription;
        }
        set
        {
            objectPlacementDescription = value;
            spaceQueryDescription = "";
        }
    }

    public bool DoesScanMeetMinBarForCompletion
    {
        get
        {
            // Only allow this when we are actually scanning
            if ((SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Scanning) ||
                (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
            {
                return false;
            }

            // Query the current playspace stats
            IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
            if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
            {
                return false;
            }
            SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

            // Check our preset requirements
            if ((stats.TotalSurfaceArea > kMinAreaForComplete) ||
                (stats.HorizSurfaceArea > kMinHorizAreaForComplete) ||
                (stats.WallSurfaceArea > kMinWallAreaForComplete))
            {
                return true;
            }
            return false;
        }
    }

    public string PrimaryText
    {
        get
        {
            // Display the space and object query results (has priority)
            if (!string.IsNullOrEmpty(SpaceQueryDescription))
            {
                return SpaceQueryDescription;
            }
            else if (!string.IsNullOrEmpty(ObjectPlacementDescription))
            {
                return ObjectPlacementDescription;
            }

            // Scan state
            if (SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
            {
                switch (SpatialUnderstanding.Instance.ScanState)
                {
                    case SpatialUnderstanding.ScanStates.Scanning:
                        // Get the scan stats
                        IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                        if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                        {
                            return "Space statistics query failed";
                        }

                        // The stats tell us if we could potentially finish
                        if (DoesScanMeetMinBarForCompletion)
                        {
                            return "When ready, air tap to finalize your playspace";
                        }
                        return "Walk around and scan in your playspace";
                    case SpatialUnderstanding.ScanStates.Finishing:
                        return "Finalizing scan (please wait)";
                    case SpatialUnderstanding.ScanStates.Done:
                        return "Scan complete";
                    default:
                        return "ScanState = " + SpatialUnderstanding.Instance.ScanState.ToString();
                }
            }
            return "";
        }
    }

    public Color PrimaryColor
    {
        get
        {
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning)
            {
                if (trackedHandsCount > 0)
                {
                    return DoesScanMeetMinBarForCompletion ? Color.green : Color.red;
                }
                return DoesScanMeetMinBarForCompletion ? Color.yellow : Color.white;
            }

            // Special case processing & 
            return (!string.IsNullOrEmpty(SpaceQueryDescription) || !string.IsNullOrEmpty(ObjectPlacementDescription)) ?
                (PrimaryText.Contains("processing") ? new Color(1.0f, 0.0f, 0.0f, 1.0f) : new Color(1.0f, 0.7f, 0.1f, 1.0f)) :
                new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public string DetailsText
    {
        get
        {
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.None)
            {
                return "";
            }

            // Scanning stats get second priority
            if ((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                (SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
            {
                IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                {
                    return "Playspace stats query failed";
                }
                SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

                // Start showing the stats when they are no longer zero
                if (stats.TotalSurfaceArea > kMinAreaForStats)
                {
                    string subDisplayText = string.Format("Total Area={0:0.0}, Horiz={1:0.0}, Wall={2:0.0}", stats.TotalSurfaceArea, stats.HorizSurfaceArea, stats.WallSurfaceArea);
                    subDisplayText += string.Format("\nFloorCells={0}, CeilingCells={1}, Platform Cells={2}", stats.NumFloor, stats.NumCeiling, stats.NumPlatform);
                    subDisplayText += string.Format("\nPaint Mode={0}, Seen Cells={1}, Not Seen={2}", stats.CellCount_IsPaintMode, stats.CellCount_IsSeenQualtiy_Seen + stats.CellCount_IsSeenQualtiy_Good, stats.CellCount_IsSeenQualtiy_None);
                    return subDisplayText;
                }
                return "";
            }
            return "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        Update_DebugDisplay(Time.deltaTime);
        Update_KeyboardInput(Time.deltaTime);
    }

    private void Update_DebugDisplay(float deltaTime)
    {
        // Basic checks
        if (DebugDisplay == null)
        {
            return;
        }

        // Update display text
        DebugDisplay.text = PrimaryText;
        DebugDisplay.color = PrimaryColor;
        DebugSubDisplay.text = DetailsText;
    }

    private void Update_KeyboardInput(float deltaTime)
    {
        // Toggle SurfaceMapping & CustomUnderstandingMesh visibility
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            ToggleScannedMesh();
        }
        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            ToggleProcessedMesh();
        }
    }

    private void ToggleProcessedMesh()
    {
        SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh = !SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh;
    }

    private void ToggleScannedMesh()
    {
        SpatialMappingManager.Instance.DrawVisualMeshes = !SpatialMappingManager.Instance.DrawVisualMeshes;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if ((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                !SpatialUnderstanding.Instance.ScanStatsReportStillWorking)
        {
            SpatialUnderstanding.Instance.RequestFinishScan();
        }
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {
        // If the source has positional info and there is currently no visible source
        if (eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
        {
            trackedHandsCount++;
        }
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        if (eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
        {
            trackedHandsCount--;
        }
    }
}
