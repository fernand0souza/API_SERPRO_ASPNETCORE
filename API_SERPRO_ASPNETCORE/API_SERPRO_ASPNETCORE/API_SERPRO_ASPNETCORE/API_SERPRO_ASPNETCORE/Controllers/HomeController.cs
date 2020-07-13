using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using API_SERPRO_ASPNETCORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_SERPRO_ASPNETCORE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // VARIAVEIS
        string getstring = "";

        // GET: api/Home/value
        [HttpGet("{CPF}", Name = "Get")]
        public async Task<string> GetAsync(string CPF)
        {
            var SerproToken = new SerproToken();
            var getReturn = await APIserpro(CPF, SerproToken);
            Objectlist(getReturn);
            return getstring;
        }


        public async Task<string> APIserpro(string CPF, object jsonObject)
        {
            //Token 
            try
            {

                var SerproToken = new SerproToken();
                var parametro = new
                {
                    SerproToken.access_token
                };

                var jsonContent = JsonConvert.SerializeObject(parametro);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Get server key 
                var serverKey = string.Format("Basic {0}", ""); // BASE 64 credenciais de autenticação

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://apigateway.serpro.gov.br/token?grant_type=client_credentials"))
                {

                    using (HttpClient httpClient = new HttpClient())
                    {

                        var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                        httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                        var response = await httpClient.SendAsync(httpRequest);
                        if (response != null)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            string tokenjson = JsonConvert.SerializeObject(jsonString, Formatting.Indented);

                            ConverteJSonParaObjectlist(jsonString);

                        }


                    }
                }

                var Bearer = string.Format("Bearer {0}", getstring);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, "https://apigateway.serpro.gov.br/consulta-cpf-df/v1/cpf/" + CPF))
                {

                    using (HttpClient httpClient = new HttpClient())
                    {

                        var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                        httpRequest.Headers.TryAddWithoutValidation("Authorization", Bearer);
                        var response = await httpClient.SendAsync(httpRequest);
                        if (response != null)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            string tokenjson = JsonConvert.SerializeObject(jsonString, Formatting.Indented);
                            return jsonString;
                        }


                    }
                }




            }
            catch (Exception ex)
            {
            }
            return CPF;
        }

        public string ConverteJSonParaObjectlist(string jsonString)
        {
            try
            {
                var SerproToken = new SerproToken();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SerproToken));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                SerproToken obj = (SerproToken)serializer.ReadObject(ms);
                jsonString = obj.access_token;
                getstring = jsonString;
                return getstring;
            }
            catch
            {
                throw;
            }

        }

        public string Objectlist(string jsonString)
        {
            try
            {
                var SerproToken = new SerproToken();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SerproToken));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                if (jsonString != "")
                {
                    SerproToken obj = (SerproToken)serializer.ReadObject(ms);
                    if (obj.mensagem != null)
                    {
                        getstring = "" + obj.mensagem;
                    }
                    else { getstring = "" + obj.ni + "," + obj.nascimento + "," + obj.nome + "," + obj.situacao.codigo + "," + obj.situacao.descricao + ""; }
                }
                else
                {
                    getstring = "CPF NÃO ENCONTRADO !";
                }
                return getstring;
            }
            catch
            {
                throw;
            }

        }





    }



}
