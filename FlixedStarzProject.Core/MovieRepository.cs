using CsvHelper;
using FlixedStarzProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlixedStarzProject.Core
{
    public class MovieRepository : IMovieRepository
    {
        public async Task<List<Data>> GetMovieData()
        {
            var mydata = new List<Data>();

            var url = "https://playdata.starz.com/metadata-service/play/partner/Web/v8/blocks?playContents=map&lang=en-US&pages=BROWSE,HOME,MOVIES,PLAYLIST,SEARCH,SEARCH%20RESULTS,SERIES&includes=contentId,contentType,title,product,seriesName,seasonNumber,free,comingSoon,newContent,topContentId,properCaseTitle,categoryKeys,runtime,popularity,original,firstEpisodeRuntime,releaseYear,images,minReleaseYear,maxReleaseYear,episodeCount,detail";

            using (var client = new HttpClient())
            {

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<MovieData>(result);
                    var requiredData = data.blocks[7].playContentsById;

                    foreach (var item in requiredData)
                    {
                        var values = new Data();
                        values.Title = item.Value.title;
                        values.ReleaseYear = item.Value.releaseYear;
                        mydata.Add(values);
                    }

                    using (var writer = new StreamWriter("movies.csv"))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(mydata);
                    }

                    return mydata;
                }

                return null;

            }
        }

      

       
    }
}
