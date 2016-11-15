using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    public class TaskIO
    {
        public static void fillTaskFile(string filePath, int numberOfInput, string[] parameters)
        {
            System.IO.File.WriteAllText(filePath, numberOfInput.ToString()+"\n");
            for (int i = 0; i < numberOfInput; ++i)
            {
                System.IO.File.AppendAllText(filePath, "\n");
            }

            System.IO.File.AppendAllLines(filePath, parameters);
        }

        public static void fillDataFile(string filePath, double[] data, string[] parameters)
        {
            System.IO.File.WriteAllText(filePath, "var data = [];\nvar params = {};\n");
            for (int i = 0; i < data.Length; ++i)
            {
                System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                System.IO.File.AppendAllText(filePath, "data[" + i.ToString() + "] = " + data[i].ToString(provider) + ";\n");
            }

            for (int i = 0; i < parameters.Length; ++i)
            {
                string[] pp = parameters[i].Split(' ');
                System.IO.File.AppendAllText(filePath, "params." + pp[0] + " = " + pp[1] + ";\n");
            }
        }

        public static void parseInput(string filePath, out int numberOfInputs, out double[] input, out string[] parameters)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            string line = file.ReadLine();
            if (line == null)
            {
                numberOfInputs = 0;
                input = null;
                parameters = null;
                file.Close();
                return;
            }
            numberOfInputs = Convert.ToInt32(line);

            List<double> tmpInput = new List<double>();
            for (int i = 0; i < numberOfInputs; ++i)
            {
                line = file.ReadLine();
                if (line == null)
                {
                    numberOfInputs = 0;
                    input = null;
                    parameters = null;
                    file.Close();
                    return;
                }
                System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                tmpInput.Add(Convert.ToDouble(line, provider));
            }
            input = tmpInput.ToArray();

            List<string> tmpParams = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                tmpParams.Add(line);
            }
            parameters = tmpParams.ToArray();
            file.Close();
        }
    }
}