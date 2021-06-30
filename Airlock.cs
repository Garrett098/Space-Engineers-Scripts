namespace IngameScript
{
    public partial class Program
    {
        public class Airlock
        {
            public bool sealing = false;
            public DateTime sealTime = DateTime.Now;
            public List<IMyAirtightSlideDoor> innerDoors, outerDoors;
            public bool inOpen, outOpen;
            public IMySensorBlock sensor;
            public Program parentProgram;

            public Airlock(Program prg)
            {
                innerDoors = new List<IMyAirtightSlideDoor>();
                outerDoors = new List<IMyAirtightSlideDoor>();
                parentProgram = prg;
                parentProgram.GridTerminalSystem.GetBlocksOfType(innerDoors, door => door.CustomName.Contains("[In]")); //, door => door.CustomName.Contains("[In]"));
                parentProgram.GridTerminalSystem.GetBlocksOfType(outerDoors, door => door.CustomName.Contains("[Out]"));
                //parentProgram.Echo(innerDoors.Count().ToString());
                //parentProgram.Echo(outerDoors.Count().ToString());
                inOpen = checkOpen(innerDoors); outOpen = checkOpen(outerDoors);
            }

            public void Close()
            {
                if (!sealing)
                {
                    sealing = true;
                    sealTime = DateTime.Now.AddSeconds(2);
                    foreach (var i in innerDoors)
                    {
                        i.CloseDoor();
                        
                    }
                    inOpen = false;
                    foreach (var j in outerDoors)
                    {
                        j.CloseDoor();
                    }
                    outOpen = false;
                }
            }

            public void OpenInner()
            {
                CheckSeal();
                if (!sealing)
                    foreach (var i in innerDoors)
                    {
                        i.OpenDoor();
                    }
                    inOpen = true;
            }

            public void OpenOuter()
            {
                CheckSeal();
                if (!sealing)
                    foreach (var i in outerDoors)
                    {
                        i.OpenDoor();
                    }
                    outOpen = true;
            }

            public void CheckSeal()
            {
                if (sealing && DateTime.Now >= sealTime) sealing = false;
            }

            public bool checkOpen(List<IMyAirtightSlideDoor> doors)
            {
                foreach (var i in doors)
                {
                    if (i.Status.HasFlag(DoorStatus.Open)) inOpen = true; outOpen = true; return true;
                }

                return false;
            }
        }
    }
}
