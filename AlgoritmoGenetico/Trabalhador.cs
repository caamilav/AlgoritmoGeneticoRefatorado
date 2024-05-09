namespace AlgoritmoGenetico
{
    public class Trabalhador
    {
        public string Nome { get; set; }
        public int Tarefa { get; set; }
        public int Nota { get; set; }


        public Trabalhador Clone()
        {
            return new Trabalhador
            {
                Nome = this.Nome,
                Tarefa = this.Tarefa,
                Nota = this.Nota
            };
        }
    }
}
