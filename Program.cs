
namespace IngameScript
{
    partial class MiningRig : MyGridProgram
    {
        bool drilling = false;
        bool done = false;

        IMyBlockGroup drillBits;
        IMyBlockGroup drillPistons;

        IMyBlockGroup landingGear;
        IMyBlockGroup landingPistons;

        IMyTextPanel cns;
        List<IMyShipDrill> drills = new List<IMyShipDrill>();
        List<IMyPistonBase> pistons = new List<IMyPistonBase>();
        List<IMyLandingGear> landinggear = new List<IMyLandingGear>();
        List<IMyPistonBase> landingpistons = new List<IMyPistonBase>();

        public MiningRig()
        {
            done = bool.TryParse(Storage, out done);

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            drillBits = GridTerminalSystem.GetBlockGroupWithName("DrillBits"); //??
            drillPistons = GridTerminalSystem.GetBlockGroupWithName("DrillPistons"); //??

            landingGear = GridTerminalSystem.GetBlockGroupWithName("RigLandingGear"); //??
            landingPistons = GridTerminalSystem.GetBlockGroupWithName("LandingPistons");

            cns = GridTerminalSystem.GetBlockWithName("LCD Panel 2") as IMyTextPanel;

            drillBits.GetBlocksOfType(drills);
            drillPistons.GetBlocksOfType(pistons);

            landingGear.GetBlocksOfType(landinggear);
            landingPistons.GetBlocksOfType(landingpistons);



        }

        public void Save()
        {
            Storage = done.ToString();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument == "again")
            {
                done = false;
            }
            cns.WriteText("");
            if (!drilling & !done)
            {
                if (!drills[0].Enabled & LandingGearLocked())
                {
                    ToggleDrills();
                    //CheckLandingGears();
                    ExtendDrillPistons();
                    drilling = true;
                }
                else
                {
                    cns.WriteText("Alright let me get those landing gears\n for you bud. \n");
                    LockLandingGear();
                }
            }
            if (pistonInfo() == "2.0m")
            {
                ToggleDrills();
                drilling = false;
                RetractDrillPistons();
                done = true;
            }
            if (done & pistonInfo() == "0.0m" &
                landinggear[0].IsLocked & landinggear[1].IsLocked)
            {
                UnlockLandingGear();
                
            }
           
        }

        public void LockLandingGear()
        {
            foreach (var i in landinggear)
            {
                i.AutoLock = true;
            }
            ExtendLandingGear();
        }

        public void UnlockLandingGear()
        {
            foreach (var i in landinggear)
            {
                i.AutoLock = false;
                i.Unlock();
            }
            RetractLandingGear();
        }

        public string landingPistonInfo()
        {
            string p1 = landingpistons[0].DetailedInfo.Split(' ', '\n')[3];
            string p2 = landingpistons[1].DetailedInfo.Split(' ', '\n')[3];
            return p1 + " " + p2;
        }

        public void ExtendLandingGear()
        {
            foreach (var i in landingpistons)
            {
                i.Extend();
            }
        }

        public void RetractLandingGear()
        {
            foreach (var i in landingpistons)
            {
                i.Retract();
            }
        }

        public bool LandingGearLocked()
        {
            foreach (var i in landinggear)
            {
                if (i.IsLocked)
                {
                    return true;
                }
            }
            return false;
        }

        public string pistonInfo()
        {
            string tmp = "";
            foreach (var i in pistons)
            {
                tmp = i.DetailedInfo.Split(' ', '\n')[3];
                cns.WriteText(i.CustomName + " : " + tmp + "\n", true);
            }
            return tmp;
        }
        public void ExtendDrillPistons()
        {
            foreach (var i in pistons)
            {
                i.Extend();
            }
        }

        public void RetractDrillPistons()
        {
            foreach (var i in pistons)
            {
                i.Retract();
            }
        }
        public void ToggleDrills()
        {
            foreach (var i in drills)
            {
                if (i.Enabled)
                {
                    cns.WriteText("Turning off: " + i.CustomName + "\n", true);
                }
                else
                {
                    cns.WriteText("Turning on: " + i.CustomName + "\n", true);
                }
                i.Enabled = !i.Enabled;
            }
        }    ////////////////
    } /////////////
}
