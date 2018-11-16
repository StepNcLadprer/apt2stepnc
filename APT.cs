using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace apt2stepnc
{
    class APT
    {
        // nested classes----------------------------------------------------------------------------------

        public class MetaData
        {
            public string PartNo = "Part-";
            public string Units = "Millimeters";
            public string Multax = "ON";
            public Dictionary<int, Cutter> CutterTools = new Dictionary<int, Cutter>();
            public List<MachineGroup> MachineGroups = new List<MachineGroup>();
        }

        public class MachineGroup
        {
            public int ID = -1;
            public List<ToolpathGroup> Operations = new List<ToolpathGroup>();
        }

        public class ToolpathGroup
        {
            public int ID = -1;
            public int Tool = -1;
            public List<MachiningData> MachiningData = new List<MachiningData>();
        }

        public class Cutter
        {
            public int ID = -1;
            public double Diameter = 0.0;
            public double Length = 0.0;
            public string Name = "Deftool";
        }

        public class Point
        {
            private double x = 0.0; public double X { get { return x; } set { x = value; } }
            private double y = 0.0; public double Y { get { return y; } set { y = value; } }
            private double z = 0.0; public double Z { get { return z; } set { z = value; } }
            public Point() { }
            public Point(double new_x, double new_y, double new_z) { x = new_x; y = new_y; z = new_z; }
        }

        public class MachiningData { }

        public class Rapid : MachiningData { }

        public class GoTo : MachiningData
        {
            public Point point = new Point();
            public GoTo() { }
            public GoTo(double new_x, double new_y, double new_z) { point.X = new_x; point.Y = new_y; point.Z = new_z; }
            public void parseGoto(string line)
            {
                string[] substrings = Regex.Split(line.Substring("GOTO/".Length), ",");
                point.X = Convert.ToDouble(substrings[0].Replace(" ", string.Empty));
                point.Y = Convert.ToDouble(substrings[1].Replace(" ", string.Empty));
                point.Z = Convert.ToDouble(substrings[2].Replace(" ", string.Empty));
            }
        }

        public class Circle : MachiningData
        {
            public Point endpoint = new Point();
            public Point center = new Point();
            public double radius; public double Radius { get { return radius; } set { radius = value; } }
            public bool direction; public bool Direction { get { return direction; } set { direction = value; } }
            public void parseCircle(Indirv indirv, Point startpoint, string line1, string line2, string line3)
            {
                //getting center
                string[] substrings = Regex.Split(line1.Substring(line1.IndexOf("CIRCLE/") + "CIRCLE/".Length), ",");
                center.X = Convert.ToDouble(substrings[0].Replace(" ", string.Empty));
                center.Y = Convert.ToDouble(substrings[1].Replace(" ", string.Empty));
                center.Z = Convert.ToDouble(substrings[2].Replace(" ", string.Empty));

                //getting radius
                radius = Convert.ToDouble(line2.Substring(0, line2.IndexOf(")") - 1).Replace(" ", string.Empty));

                //getting end point
                string[] substring2 = line3.Split(',');
                endpoint.X = Convert.ToDouble(substring2[0].Replace(" ", string.Empty));
                endpoint.Y = Convert.ToDouble(substring2[1].Replace(" ", string.Empty));
                endpoint.Z = Convert.ToDouble(substring2[2].Replace(" ", string.Empty).Replace(")", string.Empty));

                //determines the direction of movement
                if (indirv.i <= 0 && indirv.j < 0)
                {
                    if (center.X < startpoint.X && center.Y >= startpoint.Y)
                    {
                        direction = false;//CW
                    }
                    else if (center.X > startpoint.X && center.Y <= startpoint.Y)
                    {
                        direction = true;//CCW
                    }
                }

                if (indirv.i < 0 && indirv.j >= 0)
                {
                    if (center.X >= startpoint.X && center.Y > startpoint.Y)
                    {
                        direction = false;//CW
                    }
                    else if (center.X <= startpoint.X && center.Y < startpoint.Y)
                    {
                        direction = true;//CCW
                    }
                }

                if (indirv.i >= 0 && indirv.j > 0)
                {
                    if (center.X > startpoint.X && center.Y <= startpoint.Y)
                    {
                        direction = false;//CW
                    }
                    else if (center.X < startpoint.X && center.Y >= startpoint.Y)
                    {
                        direction = true;//CCW
                    }
                }

                if (indirv.i > 0 && indirv.j <= 0)
                {
                    if (center.X <= startpoint.X && center.Y < startpoint.Y)
                    {
                        direction = false;//CW
                    }
                    else if (center.X >= startpoint.X && center.Y > startpoint.Y)
                    {
                        direction = true;//CCW
                    }
                }
            }
        }

        public class Feedrate : MachiningData
        {
            private double fedrat = 0.0; public double Fedrat { get { return fedrat; } set { fedrat = value; } }
            private string unit = "IPM"; public string Unit { get { return unit; } set { unit = value; } }
            public Feedrate() { }
            public Feedrate(double new_fedrat, string new_unit) { fedrat = new_fedrat; unit = new_unit; }
            public void parseFedrat(string line)
            {
                string[] substrings = Regex.Split(line.Substring("FEDRAT/".Length), ",");
                fedrat = Convert.ToDouble(substrings[1].Replace(" ", string.Empty));
                unit = substrings[0].Replace(" ", string.Empty);
            }
        }

        public class SpindleSpeed : MachiningData
        {
            private double spindleSpeed; public double Spindlespeed { get { return spindleSpeed; } set { spindleSpeed = value; } }
            private string direction = "CLW"; public string Direction { get { return direction; } set { direction = value; } }
            private string unit = "RPM"; public string Unit { get { return unit; } set { unit = value; } }
            public SpindleSpeed() { }
            public SpindleSpeed(double new_spindleSpeed, string new_direction, string new_unit)
            {
                unit = new_unit;
                direction = new_direction;
                spindleSpeed = new_spindleSpeed;
            }
            public void parseSpindleSpeed(string line)
            {
                string[] substrings = line.Split(',');
                spindleSpeed = Convert.ToDouble(substrings[1].Replace(" ", string.Empty));
                direction = substrings[2].Replace(" ", string.Empty);
                unit = substrings[0].Replace(" ", string.Empty);
            }
        }

        public class Coolant : MachiningData
        {
            public bool activation = true; //false -> OFF, true -> ON;FLOOD
            public void parseCoolant(string line)
            {
                string foo = line.Substring("COOLNT/".Length).Replace(" ",string.Empty);
                if(foo == "OFF")
                    activation = false;
            }
        }

        public class Indirv : MachiningData
        {
            public double i;
            public double j;
            public double k;
            public void parseIndirv(string line)
            {
                string[] substrings = Regex.Split(line.Substring("INDIRV/".Length), ",");
                i = Convert.ToDouble(substrings[0].Replace(" ", string.Empty));
                j = Convert.ToDouble(substrings[1].Replace(" ", string.Empty));
                k = Convert.ToDouble(substrings[2].Replace(" ", string.Empty));
            }
        }

        //public-----------------------------------------------------------------------------
        public MetaData MastercamData = new MetaData();

        public bool ReadMastercamCLFile(String filename)
        {
            Queue<string> lines = new Queue<string>(File.ReadAllLines(filename));
            if (lines.Count <= 0)
            {
                MessageBox.Show("Error: empty file!");
                return false;
            }

            MastercamData = parseMastercamAPT(lines);

            if (MastercamData.MachineGroups.Count > 0)
                return true;

            return false;
        }

        public void WriteSTEPNC(MetaData metadata, string outname)
        {
            STEPNCLib.AptStepMaker stnc = new STEPNCLib.AptStepMaker();

            stnc.NewProjectWithCCandWP(metadata.PartNo, 1, "Main Workplan");//make new project

            if (metadata.Units == "IN" || metadata.Units == "Inches")
                stnc.Inches();
            else
                stnc.Millimeters();

            if (metadata.Multax == "ON")
                stnc.MultaxOn();

            Cutter[] tools = new Cutter[metadata.CutterTools.Keys.Count];
            metadata.CutterTools.Values.CopyTo(tools, 0);

            for (int i = 0; i < tools.Length; i++)//define all tools
            {
                stnc.DefineTool(tools[i].Diameter, tools[i].Diameter/2, 10.0, 10.0, 1.0, 0.0, 45.0);
            }

            foreach (MachineGroup machineGroup in metadata.MachineGroups)
            {
                stnc.NestWorkplan("Machine Group-" + machineGroup.ID.ToString());

                foreach (ToolpathGroup toolpahtGroup in machineGroup.Operations)
                {
                    stnc.LoadTool(toolpahtGroup.Tool);//load tool associated with the current operation
                    stnc.Workingstep("WS-" + toolpahtGroup.ID.ToString());

                    foreach (MachiningData mchData in toolpahtGroup.MachiningData)
                    {
                        if (mchData is Rapid)
                        {
                            stnc.Rapid();
                        }
                        if (mchData is GoTo)
                        {
                            GoTo temp = mchData as GoTo;
                            stnc.GoToXYZ("point", temp.point.X, temp.point.Y, temp.point.Z);
                        }
                        if (mchData is Circle)
                        {
                            Circle temp = mchData as Circle;
                            stnc.ArcXYPlane("arc", temp.endpoint.X, temp.endpoint.Y, temp.endpoint.Z, temp.center.X, temp.center.Y, temp.center.Z, temp.radius, temp.direction);
                        }
                        if (mchData is Feedrate)
                        {
                            Feedrate temp = mchData as Feedrate;
                            stnc.Feedrate(temp.Fedrat);
                        }
                        if (mchData is SpindleSpeed)
                        {
                            SpindleSpeed temp = mchData as SpindleSpeed;
                            stnc.SpindleSpeed(temp.Spindlespeed);
                        }
                        if (mchData is Coolant)
                        {
                            Coolant temp = mchData as Coolant;
                            if (temp.activation)
                                stnc.CoolantOn();
                            else
                                stnc.CoolantOff();
                        }
                    }
                }
                stnc.EndWorkplan();
            }
            stnc.SaveAsP21(outname);
            return;
        }

        //private-----------------------------------------------------------------
        private MetaData parseMastercamAPT(Queue<string> lines)
        {
            string line;
            Indirv current_indirv = new Indirv();
            Point current_position = new Point(0.0, 0.0, 0.0);
            int current_tool = -1;
            MetaData metadata = new MetaData();

            while (!lines.Peek().Contains("FINI"))//data parsing
            {
                if (lines.Peek().Contains("$$Machine Group-"))
                {
                    MachineGroup mg = new MachineGroup();
                    mg.ID = Convert.ToInt32(lines.Peek().Substring("$$Machine Group-".Length));

                    while (!lines.Peek().Contains("FINI") || (lines.Peek().Contains("$$Machine Group-") && (Convert.ToInt32(lines.Peek().Substring("$$Machine Group-".Length)) == mg.ID)))
                    {
                        lines.Dequeue();
                        ToolpathGroup tpg = new ToolpathGroup();
                        tpg.ID++;
                        
                        while (!lines.Peek().Contains("$$Machine Group-") && !lines.Peek().Contains("FINI"))
                        {
                            line = lines.Dequeue();
                            if (line.Contains("PARTNO/"))
                            {
                                metadata.PartNo = line.Substring("PARTNO/".Length);
                                continue;
                            }

                            if (line.Contains("UNITS/"))
                            {
                                metadata.Units = line.Substring("UNITS/".Length);
                                metadata.Units = metadata.Units.Replace(" ", string.Empty);
                                continue;
                            }

                            if (line.Contains("MULTAX/"))
                            {
                                metadata.Multax = line.Substring("MULTAX/".Length);
                                metadata.Multax = metadata.Multax.Replace(" ", string.Empty);
                                continue;
                            }

                            //if (line.Contains("MACHIN/"))
                            //{
                            //    string[] substrings = Regex.Split(line.Substring("MACHIN/".Length), ",");
                            //    metadata.Machin = substrings[0].Replace(" ", string.Empty);
                            //    continue;
                            //}

                            if (line.Contains("CUTTER/"))
                            {
                                Cutter tool = new Cutter();
                                string[] substrings = Regex.Split(line.Substring("CUTTER/".Length), ",");
                                tool.Diameter = Convert.ToDouble(substrings[0].Replace(" ", string.Empty));
                                //tpg.tpgtool.length = Convert.ToDouble(substrings[1].Replace(" ", string.Empty));

                                line = lines.Dequeue();
                                tool.Name = line.Substring("TPRINT/".Length);

                                line = lines.Dequeue();
                                string[] substrings2 = Regex.Split(line.Substring("LOAD/".Length), ",");
                                current_tool = tpg.Tool = tool.ID = Convert.ToInt32(substrings2[1].Replace(" ", string.Empty).Replace(".", string.Empty));

                                if (!metadata.CutterTools.ContainsKey(tool.ID))
                                    metadata.CutterTools.Add(tool.ID, tool);

                                continue;
                            }

                            if (line.Contains("RAPID"))
                            {
                                tpg.MachiningData.Add(new Rapid());
                                continue;
                            }

                            if (line.Contains("GOTO/"))
                            {
                                GoTo gotoxyz = new GoTo();
                                gotoxyz.parseGoto(line);
                                tpg.MachiningData.Add(gotoxyz);
                                current_position = gotoxyz.point;
                                continue;
                            }

                            if (line.Contains("INDIRV/"))
                            {
                                Indirv indirv = new Indirv();
                                indirv.parseIndirv(line);
                                tpg.MachiningData.Add(indirv);
                                current_indirv = indirv;
                                continue;
                            }

                            if (line.Contains("CIRCLE/"))
                            {
                                Circle circle = new Circle();
                                circle.parseCircle(current_indirv, current_position, line, lines.Dequeue(), lines.Dequeue());
                                tpg.MachiningData.Add(circle);
                                current_position = circle.endpoint;
                                continue;
                            }

                            if (line.Contains("FEDRAT/"))
                            {
                                Feedrate fedrat = new Feedrate();
                                fedrat.parseFedrat(line);
                                tpg.MachiningData.Add(fedrat);
                                continue;
                            }

                            if (line.Contains("SPINDL/"))
                            {
                                SpindleSpeed spindle = new SpindleSpeed();
                                spindle.parseSpindleSpeed(line);
                                tpg.MachiningData.Add(spindle);
                                continue;
                            }

                            if(line.Contains("COOLNT/"))
                            {
                                Coolant coolant = new Coolant();
                                coolant.parseCoolant(line);
                                tpg.MachiningData.Add(coolant);
                                continue;
                            }

                        }
                        if (tpg.Tool == -1)
                            tpg.Tool = current_tool;

                        mg.Operations.Add(tpg);//add toolpath to list
                    }
                    metadata.MachineGroups.Add(mg);//add current machine group to list
                }
                else
                {
                    lines.Dequeue();
                }
            }
            lines.Dequeue();
            return metadata;
        }

    }
}
