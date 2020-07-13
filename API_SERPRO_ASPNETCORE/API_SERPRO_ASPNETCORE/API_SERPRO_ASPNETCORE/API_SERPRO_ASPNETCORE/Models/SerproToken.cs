using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SERPRO_ASPNETCORE.Models
{
    public class SerproToken
    {
        public string access_token { get; set; }
        public string ni { get; set; }
        public string nome { get; set; }
        public string nascimento { get; set; }
        public situacao situacao { get; set; }
        public string mensagem { get; set; }
    }

    public class situacao
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

}
