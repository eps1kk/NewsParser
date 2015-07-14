using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace NewsParser
{
    
        class ParserItem
        {

            public ParserItem()
            {

            }
            public string symbol;
            public string news;
            public string forecast;
            public string actual;
            public string previous;
            public string volatiled;
            public string time;
            public bool reverse = false;
        }
        class Filter
        {
            private List<string> mSymbols = new List<string>();
            private List<string> mNews = new List<string>();
            private string mVolatile;
            private List<string> mNewsTime = new List<string>();
            public Filter()
            {
                // Emptry constructor
            }
            public List<string> getNews() 
            {
                return mNews;
            }
            public void addNews(string news) 
            {
                mNews.Add(news); 
            }
            public void removeNews(string news)
            {
                mNews.Remove(news);
            }
            public List<string> getSymbols()
            {
                return mSymbols;
            }
            public void addSymbol(string symbol)
            {
                mSymbols.Add(symbol);
            }
            public void removeSymbol(string symbol)
            {
                mSymbols.Remove(symbol);
            }
            public void setNewsTime(string time, string news)
            {
                mNewsTime.Add(time + ";" + news);
            }
            public List<string> getNewsTime()
            {
                return mNewsTime;
            }

        }
        class Parser
        {
            private Filter mFilter;
            private string mItemBeginBlock = "<tr id=\"eventRowId_";
            private string mNewsTimeBeginBlock = "first left time";
            private string mSymbolBeginBlock = "left flagCur noWrap";
            private string mVolatileBeginBlock = "left textNum sentiment";
            private string mNewsBeginBlock = "left event";
            private string mActualBeginBlock = "bold act";
            private string mForecastBeginBlock = "fore event";
            private string mPreviousBeginBlock = "prev black";
            public Parser(Filter filter)
            {
                if (filter != null)
                {
                    mFilter = filter;
                }
                else
                {
                    throw new Exception("Filter doesn't initialized!");
                }
            }
            public List<ParserItem> parse()
            {
                List<ParserItem> parsedItems = new List<ParserItem>();
                // write parse this
                string response = getHTMLresponse();
                //System.IO.File.WriteAllText(@"E:\test.txt", response);
                List<string> blocks = new List<string>();
                string block = "";
                int j = 0;
                for (int i = 0; i < response.Length;)
                {
                    i = response.IndexOf(mItemBeginBlock, i);
                    if (i < 0) break;
                    j = response.IndexOf("</tr>", i);      
                    block = response.Substring(i, j - i);
                    blocks.Add(block);
                    i = j;
                } // Разбили страницу на блоки <tr> с событиями
                ParserItem parserItem = new ParserItem();
                string tempString = "";
                string currentNews = "";
                foreach(string item in blocks)
                {
                    int i = 0;
                    i = item.IndexOf(mNewsBeginBlock);
                    if (i > 0)
                    {
                        Console.WriteLine("tt1");
                        i = item.IndexOf(">", i) + 1;
                        i = item.IndexOf(">", i) + 1;
                        currentNews = item.Substring(i, item.IndexOf("</", i) - i);
                    }
                    i = item.IndexOf(mNewsTimeBeginBlock);
                    if (i > 0)
                    {
                        Console.WriteLine("tt11");
                        i = item.IndexOf(">", i) + 1;
                        tempString = item.Substring(i, item.IndexOf("</", i) - i);
                        mFilter.setNewsTime(tempString, currentNews);
                        parserItem.news = currentNews;
                        parserItem.time = tempString;
                    }
                    i = item.IndexOf(mSymbolBeginBlock);
                    if (i > 0)
                    {
                        Console.WriteLine("tt2");
                        i = item.IndexOf(">", i) + 1;
                        i = item.IndexOf(">", i) + 1;
                        i = item.IndexOf(">", i) + 1;
                        parserItem.symbol = item.Substring(i, item.IndexOf("<", i) - i);             
                    }
                    i = item.IndexOf(mVolatileBeginBlock);
                    if (i > 0)
                    {
                        Console.WriteLine("tt3");
                        i = item.IndexOf("title", i) + 7;
                        parserItem.volatiled = item.Substring(i, item.IndexOf("\"", i) - i);                    
                    }
                    //Console.WriteLine(parserItem.volatiled + " =========== " + parserItem.symbol);
                }
                return parsedItems;
            }

            private void cutParsedItems(List<ParserItem> parsedItems)
            {
                throw new NotImplementedException("Ну то что надо отработало");
            }
            private string getHTMLresponse()
            {   
                StringBuilder sB = new StringBuilder();
                string uri = "http://ru.investing.com";
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip.deflate,sdch");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36";
                request.Accept = "text/html";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                WebHeaderCollection requestHeaders = request.Headers;
                requestHeaders.Add("Accept-Language:ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                //requestHeaders.Add("Accept:text/html");
                //requestHeaders.Add("User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36");
                requestHeaders.Add("Cookie:__gads=ID=c6d164b2c519e5ef:T=1435221111:S=ALNI_MZ-wgo_tWlWkJoTSg5LtPExvIiXUA; activeConsent-7=1.1; _VT_content_105803_1=1; _VT_content_105871_1=1; _VT_content_105927_1=1; geoC=RU; fpros_popup=1435745154; gtmFired=OK; _ga=GA1.2.1005974067.1435221105");
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream rStream = response.GetResponseStream();
                int count = 0;
                do
                {
                    count = rStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sB.Append(Encoding.Default.GetString(buf, 0, count));
                    }
                } while (count > 0);
                return sB.ToString();
            }

        }
        class Decryptor
        {
            private List<ParserItem> highVolatile = new List<ParserItem>();
            private List<ParserItem> midVolatile = new List<ParserItem>();
            private List<ParserItem> lowVolatile = new List<ParserItem>();
            private Parser parser;
            private Filter filter;

            public Decryptor(Parser parser)
            {
                if (parser == null)
                {
                    throw new Exception("Parser didn't initialized!");
                }
                decryptParsedItems(parser.parse());
            }
            
            private  void decryptParsedItems(List<ParserItem> news)
            {
                foreach (ParserItem item in news)
                {
                    List<string> filterItems = filter.getSymbols();
                    foreach(string fItem in filterItems)
                    {
                        if (fItem.Equals(item.symbol))
                        {
                            if (item.volatiled.Equals("Высокая волатильность"))
                            {
                                highVolatile.Add(item);
                            }
                            else if (item.volatiled.Equals("Умеренная волатильность"))
                            {
                                midVolatile.Add(item);
                            }
                            else if (item.volatiled.Equals("Низкая волатильность"))
                            {
                                lowVolatile.Add(item);
                            }
                        }
                    }
                }
            }
            
            public string getAdvises()
            {
                StringBuilder advise = new StringBuilder();
                advise.Append(System.DateTime.Today.Day + "day\n");
                foreach (ParserItem item in highVolatile)
                {
                    advise.Append(getHighVolatileAdvise(item));
                }
                foreach (ParserItem item in midVolatile)
                {
                    advise.Append(getMidVolatileAdvise(item));
                }
                foreach (ParserItem item in lowVolatile)
                {
                    advise.Append(getLowVolatileAdvise(item));
                }
                return advise.ToString();
            }

            private string getLowVolatileAdvise(ParserItem item)
            {
                return getMidVolatileAdvise(item).Replace("MID", "LOW");
            }
            // Самая главная функция, основное распределение идет в ней !! 
            private string getMidVolatileAdvise(ParserItem item)
            {   
                // НУЖНА ОПТИМИЗАЦИЯ (КАЖДЫЙ РАЗ ПАРСИМ ПО НОВОЙ)!!!
                StringBuilder advise = new StringBuilder();
                string prev = item.previous.Replace(",", ".");
                double foreDouble;
                double actDouble;
                string fore = item.forecast.Replace(",", ".");
                string act = item.actual.Replace(",", ".");
                string numberFormatPattern = "\\d+[\\.,]\\d*";           
                Regex numberFormatRegex = new Regex(numberFormatPattern);
                double prevDouble = Double.Parse(numberFormatRegex.Match(prev).Value);
                if (Double.TryParse(numberFormatRegex.Match(fore).Value,out foreDouble))
                {
                   if (prevDouble < foreDouble)
                   {
                       advise.Append("BUY ACTION MID\n");
                   }
                   else if (Math.Abs(prevDouble - foreDouble) < 0.0000001)
                   {
                       advise.Append("NO ACTION\n");
                   }
                   else
                   {
                       advise.Append("SELL ACTION MID\n");
                   }
                }
                else
                {
                    advise.Append("NO ACTION\n");
                }
                if (Double.TryParse(numberFormatRegex.Match(act).Value,out actDouble))
                {
                    if (foreDouble != null)
                    {
                        if (actDouble < foreDouble)
                        {
                            advise.Clear();
                            advise.Append("SELL ACTION MID\n");
                        }
                        else if (Math.Abs(actDouble - foreDouble) < 0.0000001)
                        {
                            // заглушка
                        }
                        else
                        {
                            advise.Clear();
                            advise.Append("BUY ACTION MID\n");
                        }
                    }
                    else
                    {
                        if (actDouble < prevDouble)
                        {
                            advise.Clear();
                            advise.Append("SELL ACTION MID\n");
                        }
                        else if (Math.Abs(actDouble - prevDouble) < 0.0000001)
                        {
                            advise.Clear();
                            advise.Append("NO ACTION\n");
                        }
                        else
                        {
                            advise.Clear();
                            advise.Append("BUY ACTION MID\n");
                        }
                    }
                }
                if (item.reverse)
                {
                    advise.Replace("BUY", "SELL");
                    advise.Replace("SELL", "BUY");
                }
                advise.Insert(0, item.time + " " + item.symbol + " " + item.news + " ");
                return advise.ToString();
            }

            private string getHighVolatileAdvise(ParserItem item)
            {
                StringBuilder advise = new StringBuilder();
                advise.Append(item.time + " " + item.symbol + " " + "NO ACTION\n");
                return advise.ToString();
            }
        }
        class Writer
        {
            private string path;
            private Decryptor decryptor;
            public Writer(string path, Decryptor decryptor)
            {
                if (path.Length < 2)
                {
                    throw new Exception("Incorrect path");
                }
                if (decryptor == null)
                {
                    throw new Exception("Decryptor didn't initialized");
                }
                this.path = path;
                this.decryptor = decryptor;
            }
            // ВСЕГДА СОЗДАЁТ НОВЫЙ ФАЙЛ!!!
            public void write()
            {
                System.IO.File.WriteAllText("@" + path, decryptor.getAdvises());
            }
        }
}
