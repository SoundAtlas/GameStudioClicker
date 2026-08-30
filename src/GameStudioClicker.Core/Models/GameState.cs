using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        // Core production state
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long LinesPerSecond { get; private set; } = 0;

        public IReadOnlyList<ActiveUpgrade> ActiveUpgrades { get; }

        // Construction
        public GameState()
        {
            ActiveUpgrade MousePad = new ActiveUpgrade(
                id: "mouse_pad",
                displayName: "Mouse Pad",
                description: "Doubles lines of code per click",
                cost: 100,
                clickMultiplier: 2);

            ActiveUpgrade gamingMouse = new ActiveUpgrade(
                id: "gaming_mouse",
                displayName: "Gaming Mouse",
                description: "Doubles lines of code per click",
                cost: 400,
                clickMultiplier: 2,
                prerequisite: MousePad);

            ActiveUpgrade mechanicalKeyboard = new ActiveUpgrade(
                id: "mechanical_keyboard",
                displayName: "Mechanical Keyboard",
                description: "Doubles lines of code per click",
                cost: 700,
                clickMultiplier: 2,
                prerequisite: gamingMouse);

            ActiveUpgrade headset = new ActiveUpgrade(
                id: "headset",
                displayName: "Headset",
                description: "Doubles lines of code per click",
                cost: 800,
                clickMultiplier: 2,
                prerequisite: mechanicalKeyboard);

            ActiveUpgrade webcam = new ActiveUpgrade(
                id: "webcam",
                displayName: "Webcam",
                description: "Doubles lines of code per click",
                cost: 900,
                clickMultiplier: 2,
                prerequisite: headset);

            ActiveUpgrade externalSSD = new ActiveUpgrade(
                id: "external_ssd",
                displayName: "External SSD",
                description: "Doubles lines of code per click",
                cost: 1000,
                clickMultiplier: 2,
                prerequisite: webcam);

            ActiveUpgrade secondMonitor = new ActiveUpgrade(
                id: "second_monitor",
                displayName: "Second Monitor",
                description: "Doubles lines of code per click",
                cost: 1500,
                clickMultiplier: 2,
                prerequisite: externalSSD);

            ActiveUpgrade ultrawideMonitor = new ActiveUpgrade(
                id: "ultrawide_monitor",
                displayName: "Ultrawide Monitor",
                description: "Triples lines of code per click",
                cost: 3000,
                clickMultiplier: 3,
                prerequisite: secondMonitor);

            //9   Developer Laptop    
            //10  Developer PC    
            //11  High - End Workstation    
            //12  Home Server 
            //13  AI Workstation  
            //14  Rack Server 
            //15  Server Rack 
            //16  Server Room 
            //17  Small Data Center   
            //18  Enterprise Data Center  
            //19  Hyperscale Data Center  
            //20  Supercomputer   


            ActiveUpgrades = new List<ActiveUpgrade>
            {
                MousePad,
                gamingMouse,
                mechanicalKeyboard,
                headset,
                webcam,
                externalSSD,
                secondMonitor,
                ultrawideMonitor
            };
        }


        // Intern upgrade (passive production)
        public long InternCost { get; private set; } = 50;
        public int InternCount { get; private set; } = 0;
        public bool CanPurchaseIntern => LinesOfCode >= InternCost;

        // Junior developer upgrade (passive production)
        public long JuniorDeveloperCost { get; private set; } = 2000;
        public int JuniorDeveloperCount { get; private set; } = 0;
        public bool IsJuniorDeveloperUnlocked => InternCount >= 5;
        public bool CanPurchaseJuniorDeveloper => LinesOfCode >= JuniorDeveloperCost && IsJuniorDeveloperUnlocked;

        // Persistence

        // Creates an independent snapshot containing only values that need to be saved.
        public GameSaveData CreateSaveData()
        {
            var saveData = new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
                InternCount = this.InternCount,
                JuniorDeveloperCount = this.JuniorDeveloperCount,
            };

            foreach (var upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    saveData.PurchasedActiveUpgradeIds.Add(upgrade.Id);
                }
            }
            return saveData;
        }

        // Restores saved values and rebuilds production rates and future upgrade costs.
        public void RestoreFromSaveData(GameSaveData saveData)
        {
            // Reject null save data before reading any values from it.
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData), "Save data cannot be null.");
            }

            // Clamp persisted values in case the save file was edited or corrupted.
            LinesOfCode = Math.Max(0L, saveData.LinesOfCode);
            InternCount = Math.Max(0, saveData.InternCount);
            JuniorDeveloperCount = Math.Max(0, saveData.JuniorDeveloperCount);

            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                bool isPurchased = saveData.PurchasedActiveUpgradeIds.Contains(upgrade.Id);
                upgrade.RestorePurchaseState(isPurchased);
            }

            LinesPerClick = 1;

            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    LinesPerClick *= upgrade.ClickMultiplier;
                }
            }

            LinesPerSecond = 2 * InternCount + 20 * JuniorDeveloperCount;

            InternCost = 50;
            for (int i = 0; i < InternCount; i++)
            {
                InternCost *= 2;
            }

            JuniorDeveloperCost = 2000;
            for (int i = 0; i < JuniorDeveloperCount; i++)
            {
                JuniorDeveloperCost *= 2;
            }
        }

        public bool CanAffordUpgrade(long cost)
        {
            return LinesOfCode >= cost;
        }

        public bool CanPurchaseActiveUpgrade(ActiveUpgrade activeUpgrade)
        {
            if (ActiveUpgrades.Contains(activeUpgrade) &&
                activeUpgrade.IsAvailable &&
                CanAffordUpgrade(activeUpgrade.Cost))
            {
                return true;
            }
            return false;
        }

        public bool TryPurchaseActiveUpgrade(ActiveUpgrade activeUpgrade)
        {
            if (CanPurchaseActiveUpgrade(activeUpgrade))
            {
                LinesOfCode -= activeUpgrade.Cost;
                LinesPerClick *= activeUpgrade.ClickMultiplier;
                activeUpgrade.MarkAsPurchased();
                return true;
            }
            return false;
        }

        public bool TryPurchaseIntern()
        {
            if (CanPurchaseIntern)
            {
                LinesOfCode -= InternCost;
                InternCount += 1;
                LinesPerSecond += 2;
                InternCost *= 2;
                return true;
            }
            return false;
        }

        public bool TryPurchaseJuniorDeveloper()
        {
            if (CanPurchaseJuniorDeveloper)
            {
                LinesOfCode -= JuniorDeveloperCost;
                JuniorDeveloperCount += 1;
                LinesPerSecond += 20;
                JuniorDeveloperCost *= 2;
                return true;
            }
            return false;
        }

        // Code generation
        public void WriteCode()
        {
            LinesOfCode += LinesPerClick;
        }

        public void GeneratePassiveLines()
        {
            LinesOfCode += LinesPerSecond;
        }

        public long ApplyOfflineProgress(TimeSpan elapsed)
        {
            if (elapsed.TotalSeconds <= 0)
            {
                return 0;
            }

            // Limit offline earnings to 24 hours of passive production.
            var maxElapsed = TimeSpan.FromHours(24);
            if (elapsed > maxElapsed)
            {
                elapsed = maxElapsed;
            }

            // Fractional seconds do not produce partial lines of code.
            long wholeSeconds = (long)elapsed.TotalSeconds;

            long offlineLines = wholeSeconds * LinesPerSecond;

            LinesOfCode += offlineLines;

            return offlineLines;
        }
    }
}
