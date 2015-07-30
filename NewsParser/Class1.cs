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
        public class actualNewsItem
        {
            public actualNewsItem()
            {

            }
            public actualNewsItem(string news, string volatiled, bool reverse,string symbol)
            {
                this.news = news;
                this.volatiled = volatiled;
                this.reverse = reverse;
                this.symbol = symbol;
            }
            public string news;
            public string volatiled;
            public bool reverse;
            public string symbol;
            public int idx;
        }
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
            private List<actualNewsItem> mNews = new List<actualNewsItem>();
            public Filter()
            {
                // Emptry constructor
            }
            public List<actualNewsItem> getNews() 
            {
                return mNews;
            }
            public void setNews(List<actualNewsItem> news)
            {
                mNews = news;
            }
            public void addNews(string news, string volatiled, bool reverse, string symbol)
            {
                mNews.Add(new actualNewsItem(news, volatiled, reverse, symbol));
            }
            public void addNews(actualNewsItem item)
            {
                mNews.Add(item);
            }
            public void removeNews(actualNewsItem news)
            {
                mNews.Remove(news);
            }
            public List<string> getSymbols()
            {
                return mSymbols;
            }
            public void setSymbols(List<string> symbols)
            {
                mSymbols = symbols;
            }
            public void addSymbol(string symbol)
            {
                mSymbols.Add(symbol);
            }
            public void removeSymbol(string symbol)
            {
                mSymbols.Remove(symbol);
            }

        }
        class Parser
        {
            private string mItemPattern = "<tr id=\"eventRowId.+?</tr>";
            private string mNewsTimePattern = "<td class=\"first left time.+?</td>";
            private string mSymbolPattern = "<td class=\"left flagCur noWrap.+?</td>";
            private string mVolatilePattern = "<td class=\"left textNum sentiment.+?</td>";
            private string mNewsPattern = "<td class=\"left event.+?</td>";
            private string mActualPattern = "<td class=\"bold act.+?</td>";
            private string mForecastPattern = "<td class=\"fore event.+?</td>";
            private string mPreviousPattern = "<td class=\"prev blackFont.+?</td>";
            public Parser()
            {

            }
            public List<ParserItem> parse()
            {
                List<ParserItem> parsedItems = new List<ParserItem>();
                // write parse this
                string response = getHTMLresponse();
                //System.IO.File.WriteAllText(@"E:\test.txt", response);
                RegexOptions regOptions = RegexOptions.Singleline;
                Regex itemRegex = new Regex(mItemPattern,regOptions);
                Regex newsTimeRegex = new Regex(mNewsTimePattern,regOptions);
                Regex symbolRegex = new Regex(mSymbolPattern,regOptions);
                Regex volatileRegex = new Regex(mVolatilePattern,regOptions);
                Regex newsRegex = new Regex(mNewsPattern,regOptions);
                Regex actualRegex = new Regex(mActualPattern, regOptions);
                Regex forecastRegex = new Regex(mForecastPattern, regOptions);
                Regex previousRegex = new Regex(mPreviousPattern, regOptions);
                MatchCollection items = itemRegex.Matches(response);
                foreach(Match item in items)
                {
                    ParserItem parserItem = new ParserItem();
                    if (newsTimeRegex.IsMatch(item.Value))
                    {
                        parserItem.time = newsTimeRegex.Match(item.Value).Value;
                    }
                    if (symbolRegex.IsMatch(item.Value))
                    {
                        parserItem.symbol = symbolRegex.Match(item.Value).Value;
                    }
                    if (volatileRegex.IsMatch(item.Value))
                    {
                        parserItem.volatiled = volatileRegex.Match(item.Value).Value;
                    }
                    if (newsRegex.IsMatch(item.Value))
                    {
                        parserItem.news = newsRegex.Match(item.Value).Value;
                    }
                    if (actualRegex.IsMatch(item.Value))
                    {
                        parserItem.actual = actualRegex.Match(item.Value).Value;
                    }
                    if (forecastRegex.IsMatch(item.Value))
                    {
                        parserItem.forecast = forecastRegex.Match(item.Value).Value;
                    }
                    if (previousRegex.IsMatch(item.Value))
                    {
                        parserItem.previous = previousRegex.Match(item.Value).Value;
                    }
                    parsedItems.Add(parserItem);
                    //Console.WriteLine(parserItem.symbol);
                }
                return cutParsedItems(parsedItems);
            }

            private List<ParserItem> cutParsedItems(List<ParserItem> parsedItems)
            {   
                List<ParserItem> cutParserItems = new List<ParserItem>();
                foreach(ParserItem item in parsedItems)
                {
                    Regex cutNewsTimeRegex = new Regex("\\d+:\\d+");
                    Regex cutSymbolRegex = new Regex("[A-Z]{3}");
                    Regex cutVolatileRegex = new Regex("[А-я]+? волатильность");
                    Regex cutNewsRegex = new Regex("\\w[^>]+</a>");
                    Regex cutPrevActForeRegex = new Regex(">-?\\d+(,\\d+)?");
                    if (item.time != null && cutNewsTimeRegex.IsMatch(item.time))
                    {
                        item.time = cutNewsTimeRegex.Match(item.time).Value;
                    }
                    if (item.symbol != null && cutSymbolRegex.IsMatch(item.symbol))
                    {
                        item.symbol = cutSymbolRegex.Match(item.symbol).Value;
                    }
                    if (item.volatiled != null && cutVolatileRegex.IsMatch(item.volatiled))
                    {
                        item.volatiled = cutVolatileRegex.Match(item.volatiled).Value;
                    }
                    if (item.news != null && cutNewsRegex.IsMatch(item.news))
                    {
                        item.news = cutNewsRegex.Match(item.news).Value.Replace("</a>","");
                    }
                    if (item.previous != null && cutPrevActForeRegex.IsMatch(item.previous))
                    {
                        item.previous = cutPrevActForeRegex.Match(item.previous).Value.Replace(">", "");
                    }
                    else
                    {
                        item.previous = "";
                    }
                    if (item.forecast != null && cutPrevActForeRegex.IsMatch(item.forecast))
                    {
                        item.forecast = cutPrevActForeRegex.Match(item.forecast).Value.Replace(">", "");
                    }
                    else
                    {
                        item.forecast = "";
                    }
                    if (item.actual != null && cutPrevActForeRegex.IsMatch(item.actual))
                    {
                        item.actual = cutPrevActForeRegex.Match(item.actual).Value.Replace(">", "");
                    }
                    else
                    {
                        item.actual = "";
                    }
                    cutParserItems.Add(item);
                }
                return cutParserItems;
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
                        sB.Append(Encoding.UTF8.GetString(buf, 0, count));
                    }
                } while (count > 0);
                return sB.ToString();
            }

        }
        class Decryptor
        {
            public List<ParserItem> parsedItems = null;
            private List<ParserItem> highVolatile = new List<ParserItem>();
            private List<ParserItem> midVolatile = new List<ParserItem>();
            private List<ParserItem> lowVolatile = new List<ParserItem>();
            private Parser parser;
            private Filter filter;

            public Decryptor(Parser parser,Filter filter)
            {
                if (parser == null)
                {
                    throw new Exception("Parser didn't initialized!");
                }
                parsedItems = parser.parse();
                this.filter = filter;
                decryptParsedItems(parsedItems);
            }
            
            private  void decryptParsedItems(List<ParserItem> news)
            {
                foreach (ParserItem item in news)
                {   
                    List<string> filterSymbolItems = filter.getSymbols();
                    List<actualNewsItem> filterNewsItems = filter.getNews();
                    Regex newsCompareRegex = new Regex("(\\w+ )+");
                    foreach(string fItem in filterSymbolItems)
                    {
                        foreach (actualNewsItem fnItem in filterNewsItems)
                        {
                            if (fItem.Equals(item.symbol)  && fnItem.news.Contains(newsCompareRegex.Match(item.news).Value))
                            {
                                if (fnItem.volatiled.Equals("High"))
                                {
                                    highVolatile.Add(item);
                                }
                                else if (fnItem.volatiled.Equals("Mid"))
                                {
                                    midVolatile.Add(item);
                                }
                                else if (fnItem.volatiled.Equals("Low"))
                                {
                                    lowVolatile.Add(item);
                                }
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
                double foreDouble = Double.MaxValue;
                double actDouble;
                string fore = item.forecast.Replace(",", ".");
                string act = item.actual.Replace(",", ".");
                string numberFormatPattern = "\\d+[\\.,]\\d*";           
                Regex numberFormatRegex = new Regex(numberFormatPattern);
                double prevDouble = Double.Parse(numberFormatRegex.Match(prev).Value);
                if (numberFormatRegex.Match(fore).Success && Double.TryParse(numberFormatRegex.Match(fore).Value,out foreDouble))
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
                if (numberFormatRegex.Match(act).Success && Double.TryParse(numberFormatRegex.Match(act).Value, out actDouble))
                {
                    if (foreDouble == Double.MaxValue)
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
            public Writer(string path)
            {
                if (path.Length < 2)
                {
                    throw new Exception("Incorrect path");
                }
                this.path = path;  
            }
            // ВСЕГДА СОЗДАЁТ НОВЫЙ ФАЙЛ!!!
            public void write(string text)
            {
                System.IO.File.WriteAllText("@" + path, text);
            }
        }
}
