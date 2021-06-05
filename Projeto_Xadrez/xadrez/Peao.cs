using tabuleiro;
using tabuleiro.Enums;

namespace xadrez
{
    class Peao : Peca
    {

        public Peao(Tabuleiro tab, Cor cor) 
            : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "P";
        }

        private bool VerificarInimigoDosLados(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p != null && p.Cor != Cor;
        }

        private bool PrimeiroMovimentoInimigo(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p.QuantMov == 1;
        }

        private bool PodeMoverPara(Posicao pos)
        {
            return Tab.peca(pos) == null;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];
            Posicao pos = new Posicao(0, 0);

            if (Cor == Cor.Branco)
            {

                //acima 2x
                pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                Posicao p2 = new Posicao(Posicao.Linha - 1, Posicao.Coluna);
                if (Tab.PosicaoValida(p2) && PodeMoverPara(p2) && Tab.PosicaoValida(pos) && PodeMoverPara(pos) && QuantMov == 0)
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }


                //acima
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && PodeMoverPara(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //NE
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && VerificarInimigoDosLados(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //NO
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && VerificarInimigoDosLados(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                // #JogadaEspecial En Passant
                //NE
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && PodeMoverPara(pos))
                {
                    pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
                    if (VerificarInimigoDosLados(pos))
                    {
                        if (PrimeiroMovimentoInimigo(pos))
                        {
                            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                            mat[pos.Linha, pos.Coluna] = true;
                        }
                    }
                }

                //NO
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && PodeMoverPara(pos))
                {
                    pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
                    if (VerificarInimigoDosLados(pos))
                    {
                        if (PrimeiroMovimentoInimigo(pos))
                        {
                            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                            mat[pos.Linha, pos.Coluna] = true;
                        }
                    }
                }

            } 
            else
            {
                //acima 2x
                pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                Posicao p2 = new Posicao(Posicao.Linha + 1, Posicao.Coluna);
                if (Tab.PosicaoValida(p2) && PodeMoverPara(p2) && Tab.PosicaoValida(pos) && PodeMoverPara(pos) && QuantMov == 0)
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }


                //acima
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && PodeMoverPara(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //SE
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && VerificarInimigoDosLados(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //SO
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && VerificarInimigoDosLados(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                // #JogadaEspecial En Passant
                //SE
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && PodeMoverPara(pos))
                {
                    pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
                    if (VerificarInimigoDosLados(pos))
                    {
                        if (PrimeiroMovimentoInimigo(pos))
                        {
                            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                            mat[pos.Linha, pos.Coluna] = true;
                        }
                    }
                }

                //SO
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && PodeMoverPara(pos))
                {
                    pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
                    if (VerificarInimigoDosLados(pos))
                    {
                        if (PrimeiroMovimentoInimigo(pos))
                        {
                            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                            mat[pos.Linha, pos.Coluna] = true;
                        }
                    }
                }

            }

            return mat;
        }

    }
}
