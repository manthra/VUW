using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace VUW
{
    class ConvertDate
    {



        public void Convert(FileInfo flSource)
        {

            //Read input file
            StreamReader sr = new StreamReader(flSource.FullName.ToString());
            string strMultiLines = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();












            //<date review="1975-06-06">6 June, 1975</date>
            strMultiLines = Regex.Replace(strMultiLines, @">([^<>]+)<", delegate(Match match)
            {
                string v = match.ToString();
                return v[0] + ReplaceCustom(v.Substring(1, v.Length - 2)) + v[v.Length - 1];
            });


            //<date review="1975-06">June, 1975</date>
            strMultiLines = ReplaceCustom1(strMultiLines);
            //strMultiLines = ReplaceCustom2(strMultiLines);



            //9999-8888
            strMultiLines = Regex.Replace(strMultiLines, @">([^<>]+)<", delegate(Match match)
            {
                string v = match.ToString();
                return v[0] + ReplaceCustom3(v.Substring(1, v.Length - 2)) + v[v.Length - 1];
            });



            //9999-88
            strMultiLines = Regex.Replace(strMultiLines, @">([^<>]+)<", delegate(Match match)
            {
                string v = match.ToString();
                return v[0] + ReplaceCustom2(v.Substring(1, v.Length - 2)) + v[v.Length - 1];
            });




            //9999-9
            strMultiLines = Regex.Replace(strMultiLines, @">([^<>]+)(?!</date>)", delegate(Match match)
            {
                //string v = match.ToString();
                string x = match.Groups[1].ToString();
                return ">" + ReplaceCustom4(x) + match.Groups[2].ToString();
            });

            //9999
            strMultiLines = Regex.Replace(strMultiLines, @">([^<>]+)(?!</date>)", delegate(Match match)
            {
                //string v = match.ToString();
                string x = match.Groups[1].ToString();
                return ">" + ReplaceCustom5(x) + match.Groups[2].ToString();
            });























            //Write output file
            StreamWriter sw = new StreamWriter(flSource.FullName.ToString()+".out");
            sw.Write(strMultiLines);
            sw.Close();
            sw.Dispose();

            //return new FileInfo(flSource.FullName.ToString() + ".out");
        }


        private string ReplaceCustom(string html)
        {
            string strRetValue = html;


            //("YYYY 1. Mar, 2008 XXX", @"(\b)(\d{1,2})(\W+)([A-z][A-z][A-z]+)(\W+)(\d{4})(\b)"))
            html = Regex.Replace(html, @"(\b)(\d{1,2})(\W+)([A-z][A-z][A-z]+)(\W+)(\d{4})(\b)", delegate(Match match)
            {
                string strMnString = match.Groups[4].Value.ToString();


                strRetValue = match.ToString();

                int strMnValus = 0;

                try
                {
                    strMnValus = DateTime.Parse("1 " + strMnString + " 2008").Month;

                    if (strMnValus > 0)
                    {
                        strRetValue = "<date review=\"" + match.Groups[6].Value.ToString() + "-" + strMnValus.ToString("00") + "-" + match.Groups[2].Value.ToString().PadLeft(2, '0') + "\">" + match.ToString() + "</date>";
                    }

                }
                catch (Exception)
                {

                    //throw;
                }


                return strRetValue;
            });

            return html;
        }



        private string ReplaceCustom1(string html)
        {
            string strRetValue = html;


            //("YYYY 1. Mar, 2008 XXX", @"(\b)(\d{1,2})(\W+)(\w+)(\W+)(\d{2,4})(\b)"))
            html = Regex.Replace(html, @"(\b)([A-z][A-z][A-z]+)(\W+)(\d{4})(?!</date>)", delegate(Match match)
            {
                string strMnString = match.Groups[2].Value.ToString();


                strRetValue = match.ToString();

                int strMnValus = 0;

                try
                {
                    strMnValus = DateTime.Parse("1 " + strMnString + " 2008").Month;

                    if (strMnValus > 0)
                    {
                        strRetValue = "<date review=\"" + match.Groups[4].Value.ToString() + "-" + strMnValus.ToString("00") + "\">" + match.Groups[1].Value.ToString() + match.Groups[2].Value.ToString() + match.Groups[3].Value.ToString() + match.Groups[4].Value.ToString() + "</date>" + match.Groups[5].Value.ToString();
                    }

                }
                catch (Exception)
                {

                    //throw;
                }


                return strRetValue;
            });

            return html;
        }


        private string ReplaceCustom2(string html)
        {
            string strRetValue = html;


            //("YYYY 1. Mar, 2008 XXX", @"(\b)(\d{2})(\d{2})-(\d{2})(?!</date>)"))
            html = Regex.Replace(html, @"(\b)(\d{2})(\d{2})-(\d{2})(\b)", delegate(Match match)
            {
                try
                {
                    strRetValue = "<date review=\"" + match.Groups[2].Value.ToString() + match.Groups[3].Value.ToString() + "\">" + match.Groups[2].Value.ToString() + match.Groups[3].Value.ToString() + "</date>-<date review=\"" + match.Groups[2].Value.ToString() + match.Groups[4].Value.ToString() + "\">" + match.Groups[4].Value.ToString() + "</date>" + match.Groups[5].Value.ToString();
                }
                catch (Exception)
                {

                    // throw;
                }
                return strRetValue;
            });

            return html;
        }



        private string ReplaceCustom3(string html)
        {
            string strRetValue = html;


            //("YYYY 1. Mar, 2008 XXX", @"(\b)(\d{1,2})(\W+)(\w+)(\W+)(\d{2,4})(\b)"))
            html = Regex.Replace(html, @"(\b)([1-2][0-9][0-9][0-9])-([1-2][0-9][0-9][0-9])(\b)", delegate(Match match)
            {
                try
                {
                    strRetValue = "<date review=\"" + match.Groups[2].Value.ToString() + "\">" + match.Groups[2].Value.ToString() + "</date>-<date review=\"" + match.Groups[3].Value.ToString() + "\">" + match.Groups[3].Value.ToString() + "</date>" + match.Groups[4].Value.ToString();
                }
                catch (Exception)
                {

                    // throw;
                }
                return strRetValue;
            });

            return html;
        }




        private string ReplaceCustom4(string html)
        {
            string strRetValue = html;


            //("YYY 1. Mar, 2008 XXX", @"(\b)(\d{1,2})(\W+)(\w+)(\W+)(\d{2,4})(\b)"))
            html = Regex.Replace(html, @"(\b)([1-2][0-9][0-9])(\d{1})-(\d{1})(\b)", delegate(Match match)
            {
                try
                {
                    strRetValue = "<date review=\"" + match.Groups[2].Value.ToString() + match.Groups[3].Value.ToString() + "\">" + match.Groups[2].Value.ToString() + match.Groups[3].Value.ToString() + "</date>-<date review=\"" + match.Groups[2].Value.ToString() + match.Groups[4].Value.ToString() + "\">" + match.Groups[4].Value.ToString() + "</date>" + match.Groups[5].Value.ToString();
                }
                catch (Exception)
                {
                    // throw;
                }
                return strRetValue;
            });

            return html;
        }



        private string ReplaceCustom5(string html)
        {
            string strRetValue = html;


            //("YYYY 1. Mar, 2008 XXX", @"(\b)(\d{4})(?!</date>)"))
            html = Regex.Replace(html, @"(\b)([1-2][0-9][0-9][0-9])([^;])", delegate(Match match)
            {

                try
                {

                    strRetValue = "<date review=\"" + match.Groups[2].Value.ToString() + "\">" + match.Groups[2].Value.ToString() + "</date>" + match.Groups[3].Value.ToString();

                }
                catch (Exception)
                {

                    //throw;
                }


                return strRetValue;
            });

            return html;
        }





    }
}
