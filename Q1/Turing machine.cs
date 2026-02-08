using System;
using System.Collections.Generic;
using System.Linq;
namespace Q3
{
    public class Q3
    {
        class Transitions
        {
            public string startS = null;
            public string readS = null;
            public string writeS = null;
            public string endS = null;
            public string direction = null;
            public Transitions(string ss, string rs, string es, string wr, string j)
            {
                this.startS = ss;
                this.readS = rs;
                this.writeS = wr;
                this.endS = es;
                this.direction = j;
            }
        }
        static void Main3()
        {
            string input1 = Console.ReadLine();
            List<string> input = new List<string>();
            //split "00" :
            string s = "";
            int e=0;
            while(e != input1.Length-1)
            {
                if(input1[e] == '0' && input1[e+1] == '0')
                {
                    input.Add(s);
                    s = "";
                    e++;
                }
                else
                    s += input1[e];
                e++;
            }
            s += input1[e];
            input.Add(s);
            //Decoding : 
            List<Transitions> TRLS = new List<Transitions>();
            string endstate = "";
            foreach(var i in input)
            {
                string[] tmp = i.Split('0');
                if(endstate.Length < tmp[2].Length)
                    endstate = tmp[2];
                Transitions tr = new Transitions(tmp[0],tmp[1],tmp[2],tmp[3],tmp[4]);
                TRLS.Add(tr);
            }
            int count = int.Parse(Console.ReadLine());
            List<String> Result = new List<string>();
            for(int i=0; i<count; i++)
            {
                string str1 = Console.ReadLine();
                if(str1 == "")
                    str1 = "1";
                string[] str = str1.Split('0');
                List<string> MTape = new List<string>();
                for(int k=0; k<50; k++)
                    MTape.Add("1");//Blank
                for(int k=0; k<str.Length; k++)
                    MTape.Add(str[k]);
                for(int k=0; k<50; k++)
                    MTape.Add("1");//Blank
                string currentState = "1";
                string ArrowOfTape = MTape[50];
                int j=50;//because of Blanks
                for(int k=0; k<str.Length; k++)
                {
                    foreach(var t in TRLS)
                    {
                        if(t.startS == currentState && t.readS == MTape[j])
                        {
                            currentState = t.endS;
                            MTape[j] = t.writeS;
                            if(t.direction == "1")//Left
                            {
                                j--;
                                ArrowOfTape = MTape[j];
                            }
                            else//Right
                            {
                                j++;
                                ArrowOfTape = MTape[j];
                            }
                        }
                    }
                }
                if(currentState == endstate)
                    Result.Add("Accepted");
                else 
                {
                    int idx = 0;
                    while(currentState != endstate && idx < 50 )
                    {
                        foreach(var t in TRLS)
                        {
                            if(t.startS == currentState && t.readS == MTape[j])
                            {
                                currentState = t.endS;
                                MTape[j] = t.writeS;
                                if(t.direction == "1")//Left
                                {
                                    j--;
                                    ArrowOfTape = MTape[j];
                                }
                                else//Right
                                {
                                    j++;
                                    ArrowOfTape = MTape[j];
                                }
                            }
                        }
                        idx++;
                    }
                    if(idx < 50)
                        Result.Add("Accepted");
                    else
                        Result.Add("Rejected");
                }
            }
            foreach(var r in Result)
                System.Console.WriteLine(r);
        }
    }
}