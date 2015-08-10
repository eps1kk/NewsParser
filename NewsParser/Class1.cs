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
            public bool used = false;
            public double getAct()
            {
                try
                {
                    actual.Replace(",", ".");
                    return Double.Parse(this.actual);            
                }
                catch (Exception e)
                {
                    return Double.MaxValue;
                }
            }
            public double getFore()
            {
                try
                {
                    forecast.Replace(",", ".");
                    return Double.Parse(forecast);
                } 
                catch (Exception e)
                {
                    return Double.MaxValue;
                }
            }
            public double getPrev()
            {
                try
                {
                    previous.Replace(",", ".");
                    return Double.Parse(previous);
                }
                catch (Exception e)
                {
                    return Double.MaxValue;
                }
            }
        }
        class Filter
        {
            private List<string> mSymbols = new List<string>();
            private List<actualNewsItem> mNews = new List<actualNewsItem>();
            private Form1 mainForm;
            public Filter(Form1 form)
            {
                // Emptry constructor
                mainForm = form;
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
            public void addNews(string line)
            {
                try
                {
                    actualNewsItem item = new actualNewsItem();
                    item.symbol = mainForm.getSymbol(line);
                    item.news = mainForm.getNews(line);
                    item.volatiled = mainForm.getVolatile(line);
                    item.reverse = mainForm.getReverse(line);
                    mNews.Add(item);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            public bool equalsNews(actualNewsItem item, string line)
            {
                if (item.news.Contains(mainForm.getNews(line)) && item.symbol.Contains(mainForm.getSymbol(line)))
                    return true;
                return false;
            }
            public void removeNews(string line)
            {
                foreach (actualNewsItem item in mNews)
                {
                    if (equalsNews(item, line))
                    {
                        mNews.Remove(item);
                        break;
                    }
                }
            }
            public void updateVolatile(string line)
            {
                foreach (actualNewsItem item in mNews)
                {
                    if (equalsNews(item, line))
                    {
                        item.volatiled = mainForm.getVolatile(line);
                        break;
                    }
                }
            }
            public void updateReverse(string line)
            {
                foreach (actualNewsItem item in mNews)
                {
                    if (equalsNews(item, line))
                    {
                        item.reverse = mainForm.getReverse(line);
                        break;
                    }
                }
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
            private List<ParserItem> parsedItems = new List<ParserItem>();
            public Parser()
            {

            }
            public List<ParserItem> parse()
            {
                // write parse this
                string response = getHTMLresponse();
                if (response == null)
                    return parsedItems;
                if (parsedItems.Count > 0)
                    return parsedItems;
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
                    if (parserItem.symbol != null && parserItem.time != null)
                    {
                        parsedItems.Add(parserItem);
                    }
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
                try
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
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    return null;
                }
            }

        }
        class Decryptor
        {
            public List<ParserItem> parsedItems = new List<ParserItem>();
            private List<ParserItem> highVolatile = new List<ParserItem>();
            private List<ParserItem> midVolatile = new List<ParserItem>();
            private List<ParserItem> lowVolatile = new List<ParserItem>();
            private Filter filter;

            public Decryptor(Filter filter)
            {         
                this.filter = filter;
            }
            
            private  void decryptParsedItems(List<ParserItem> news)
            {   
                parsedItems.AddRange(highVolatile);
                parsedItems.AddRange(midVolatile);
                parsedItems.AddRange(lowVolatile);
                List<actualNewsItem> filterNewsItems = filter.getNews();             
                foreach (ParserItem parserItem in news)
                {   
                    foreach (actualNewsItem filterItem in filterNewsItems)
                    {
                        if (parserItem.symbol.Contains(filterItem.symbol) && parserItem.news.Contains(filterItem.news))
                        {   
                            if (filterItem.reverse)
                            {
                                parserItem.reverse = true;
                            }
                            bool found = false;
                            foreach (ParserItem usedItem in parsedItems)
                            {
                                if (usedItem.symbol.Contains(parserItem.symbol) && usedItem.time.Contains(parserItem.time) && usedItem.news.Contains(parserItem.news))
                                {
                                    found = true;
                                }
                            }
                            if (found)
                            {
                                continue;
                            }
                            switch (filterItem.volatiled)
                            {
                                case "High":
                                    highVolatile.Add(parserItem);
                                    break;
                                case "Mid":
                                    midVolatile.Add(parserItem);
                                    break;
                                case "Low":
                                    lowVolatile.Add(parserItem);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            
            public string getAdvises(List<ParserItem> news)
            {
                decryptParsedItems(news);
                StringBuilder advise = new StringBuilder();
                string action;
                foreach (ParserItem item in highVolatile)
                {
                    action = getHighVolatileAdvise(item);
                    if (action.Length > 0)
                    {
                        advise.Append(item.time + " " + item.symbol + " " + action + "\r\n");
                    }
                }
                foreach (ParserItem item in midVolatile)
                {   
                    action = getMidVolatileAdvise(item);
                    if (action.Length > 0)
                    {
                        advise.Append(item.time + " " + item.symbol + " " + action + "\r\n");
                    }
                }
                foreach (ParserItem item in lowVolatile)
                {
                    action = getLowVolatileAdvise(item);
                    if (action.Length > 0)
                    {
                        advise.Append(item.time + " " + item.symbol + " " + action + "\r\n");
                    }
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
                StringBuilder advise = new StringBuilder();
                // Отдаёт распарсенные значения из ParserItem, если значения нет, присваивается - MAX_VALUE;
                double prev = item.getPrev(); 
                double act = item.getAct();
                double fore = item.getFore();
                if (item.used)
                {
                    return advise.ToString();
                }
                if (prev == Double.MaxValue)
                {
                    return advise.ToString();
                }
                if (act == Double.MaxValue)
                {
                    return advise.ToString();
                }
                if (fore == Double.MaxValue)
                {
                    if (Math.Abs(act - prev) < 0.0000001)
                    {
                        advise.Append("NO ACTION");
                    }
                    else if (act < prev)
                    {
                        advise.Append("SELL ACTION");
                    }
                    else
                    {
                        advise.Append("BUY ACTION");
                    }
                }
                else
                {
                    if (Math.Abs(fore - act) < 0.0000001)
                    {
                        if (Math.Abs(act - prev) < 0.0000001)
                        {
                            advise.Append("NO ACTION");
                        }
                        else if (act < prev)
                        {
                            advise.Append("SELL ACTION");
                        }
                        else
                        {
                            advise.Append("BUY ACTION");
                        }
                    }
                    else if (act < fore)
                    {
                        advise.Append("SELL ACTION");
                    }
                    else
                    {
                        advise.Append("BUY ACTION");
                    }
                }
                if (item.reverse)
                {
                    if (advise.ToString().Contains("BUY"))
                    {
                        advise.Replace("BUY", "SELL");
                    }
                    else if (advise.ToString().Contains("SELL"))
                    {
                        advise.Replace("SELL", "BUY");
                    }
                }
                item.used = true;
                return advise.ToString().Insert(0, "MID ");
            }

            private string getHighVolatileAdvise(ParserItem item)
            {
                StringBuilder advise = new StringBuilder();
                if (!item.used)
                {
                    advise.Append("NO ACTION");
                }
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
                    throw new Exception("Incorrect advisePath");
                }
                this.path = path;  
            }
            public void write(string text)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(path, true);
                    sw.WriteLine(text);
                    sw.Close();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
        }
}
