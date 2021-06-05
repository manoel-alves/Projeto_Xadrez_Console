using tabuleiro.Enums;

namespace tabuleiro
{
    abstract class Peca
    {

        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QuantMov { get; protected set; }
        public Tabuleiro Tab { get; set; }

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.Posicao = null;
            Tab = tab;
            this.Cor = cor;
            QuantMov = 0;
        }

        public void IncrementarQuantMov()
        {
            QuantMov++;
        }

        public void DecrementarQuantMov()
        {
            QuantMov--;
        }

        public bool ValidarMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();

            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public bool VerificarMovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
            /*bool[,] mat = MovimentosPossiveis();

            if (mat[destino.Linha, destino.Coluna])
            {
                return true;
            }
            return false;*/
        }

        public abstract bool[,] MovimentosPossiveis();

    }
}
