using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FirstApp
{
    public class Robbery
    {
       public int year { get; set; } public int Property { get; set; }  public int Vehicle { get; set; }  public int Statesuproperty { get; set; }
    }
    public class RobberyCore
    {
        public int Date { get; set; }   public int NrPY { get; set; }   public int NbPY { get; set; }
    }
    public class Piechart
    {
        public string Heading { get; set; }     public int value { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int s = 0;
            int[] NrPY = new int[16];// no. of robbery per year array of 16 year
            int[] NbPY = new int[16];// no. of burglary per year array of 16 year
            int[] Property = new int[16];//array that store the no. of criminal damage to property in respective year
            int[] Vehicle = new int[16];//array that store the no. of criminal damage to vehicle in respective year
            int[] Statesupproperty = new int[16];//array that store the no. of criminal damage to statesupproperty in respective year
            int[] Subcategorycrimevalue = new int[14];//array that store the no. of robbery particular category in respective year
            Object[] barchartobject = new Object[16];//array that store the Object of barchart that is to changed into json
            Object[] differentvaluemultichart = new Object[16];//array that store the Object of multi chart that is to changed into json
            Dictionary<String, int> PieChartdata = new Dictionary<String, int>();//Dictionary that store the robbery category wise data in string and value
            StreamReader sr = new StreamReader(new FileStream("C:/Users/Training/Downloads/crimedata.csv", FileMode.OpenOrCreate));//open stream reader to read the file crimedata.csv
            StreamWriter sw = new StreamWriter(new FileStream("D:/d3/multilinechart.json", FileMode.OpenOrCreate));//open stream writer to write the json of multiline chart
            StreamWriter sw1 = new StreamWriter(new FileStream("D:/d3/stackedbarchart.json", FileMode.OpenOrCreate));//open stream writer to write the json of stacked bar chart chart
            StreamWriter sw2 = new StreamWriter(new FileStream("D:/d3/piechart.json", FileMode.OpenOrCreate));//open stream writer to write the json of piechart chart
            try
            {
                while (!sr.EndOfStream)
                {
                    string reader = sr.ReadLine();
                    if (reader.Contains("ROBBERY") || reader.Contains("BURGLARY") || reader.Contains("CRIMINAL DAMAGE"))//logic to filter data
                    {
                        string[] valueinitial = reader.Split('"');//resolving the comma in between column problem
                        if (valueinitial.Length > 1) valueinitial[1] = valueinitial[1].Replace(",", "*");
                        reader = "";
                        foreach (var a in valueinitial) { reader += a; }
                        string[] value = reader.Split(',');
                        value[1] = value[1].Replace("*", ",");
                        Int32 x = 0;
                        if (Int32.TryParse(value[17], out x))//parsing value of year to compare
                        {
                            if (x >= 2001 && x <= 2016)
                            {
                                int a = x - 2001;
                                switch (value[5])
                                {
                                    case "ROBBERY":
                                        NrPY[a] += 1;
                                        if (!PieChartdata.ContainsKey(value[6]))
                                            PieChartdata.Add(value[6], 1);
                                        else
                                            ++PieChartdata[value[6]];
                                        break;
                                    case "BURGLARY": NbPY[a] += 1; break;
                                    case "CRIMINAL DAMAGE":
                                        switch (value[6])
                                        {
                                            case "TO PROPERTY": Property[a] += 1; break;
                                            case "TO VEHICLE": Vehicle[a] += 1; break;
                                            case "TO STATE SUP PROP": Statesupproperty[a] += 1; break;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                Object[] differentvaluepiechart = new Object[PieChartdata.Keys.Count];
                foreach (KeyValuePair<string, int> pair in PieChartdata) {differentvaluepiechart[s++]= new Piechart { Heading = pair.Key, value = pair.Value }; }// changing the values of dictionary to objects and filling it in array
                for (int j = 0; j < 16; j++)//writting the file into respective files
                {
                    differentvaluemultichart[j] = new RobberyCore { Date = 2001+j,NrPY=NrPY[j],NbPY=NbPY[j] };
                    barchartobject[j] = new Robbery { year = 2001+j ,Property = Property[j], Vehicle = Vehicle[j], Statesuproperty = Statesupproperty[j] };
                }
                var multichartt = JsonConvert.SerializeObject(differentvaluemultichart, Formatting.Indented);
                var barchartt = JsonConvert.SerializeObject(barchartobject, Formatting.Indented);
                string piechart = JsonConvert.SerializeObject(differentvaluepiechart, Formatting.Indented);
                sw.WriteLine("{"+"\"multiline\" :"+multichartt); sw.Flush();
                sw1.WriteLine(barchartt); sw1.Flush();
                sw2.WriteLine(piechart);sw2.Flush();
            }
            catch (Exception e)//execption handling
            {
                Console.WriteLine("check the loops and typo error if not fixed then contact me : dhiru");
                Console.WriteLine(e.Message);
            }
        }
    }
}
