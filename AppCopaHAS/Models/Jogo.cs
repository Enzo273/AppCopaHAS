using System;
using System.Collections.Generic;

namespace AppCopaHAS.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public int EstadioId { get; set; }
        public DateTime DataHora { get; set; }

        // Passo 1 – Exercício 16: lista de seleções participantes do jogo
        public List<JogoSelecao> JogoSelecoes { get; set; } = new List<JogoSelecao>();
    }
}