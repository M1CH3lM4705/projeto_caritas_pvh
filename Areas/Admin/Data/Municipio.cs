

using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace ProjetoBetaAutenticacao.Areas.Admin.Data
{
    public class Municipio
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Municipio BuscarCidade(string Id)
        {
            var content = new Municipio();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://servicodados.ibge.gov.br/api/v1/localidades/municipios/");
                var responseTask = client.GetAsync(Id);
                responseTask.Wait();

                var result = responseTask.Result;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var readJob = result.Content.ReadAsStringAsync().Result;
                        content = JsonConvert.DeserializeObject<Municipio>(readJob);

                        
                    }
                }
                catch (Exception)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return content;
            }
        }
    }

}