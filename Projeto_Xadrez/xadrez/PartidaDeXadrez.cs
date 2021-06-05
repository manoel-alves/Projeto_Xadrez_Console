using Projeto_Xadrez;
using tabuleiro;
using tabuleiro.Enums;
using tabuleiro.Exceptions;
using System.Collections.Generic;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }
        private Peca VulneravelEnPassant;
        public bool promocao { get; private set; }


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            Terminada = false;
            Xeque = false;
            promocao = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQuantMov();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQuantMov();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQuantMov();
                tab.colocarPeca(T, destinoT);
            }

            // #jogada especial En Passant
            if (p is Peao && destino.Coluna != origem.Coluna && pecaCapturada == null)
            {
                if (p.Cor == Cor.Branco)
                {
                    Posicao PeaoInimigo;
                    // pela Direita
                    if (destino.Coluna == origem.Coluna + 1)
                    {
                        PeaoInimigo = new Posicao(origem.Linha, origem.Coluna + 1);

                    }
                    // pela esquerda
                    else
                    {
                        PeaoInimigo = new Posicao(origem.Linha, origem.Coluna - 1);

                    }
                    Peca P = tab.RetirarPeca(PeaoInimigo);
                    Capturadas.Add(P);
                }
                else
                {
                    Posicao PeaoInimigo;
                    // pela Direita
                    if (destino.Coluna == origem.Coluna + 1)
                    {
                        PeaoInimigo = new Posicao(origem.Linha, origem.Coluna + 1);

                    }
                    // pela esquerda
                    else
                    {
                        PeaoInimigo = new Posicao(origem.Linha, origem.Coluna - 1);

                    }
                    Peca P = tab.RetirarPeca(PeaoInimigo);
                    Capturadas.Add(P);
                }
            }

            return pecaCapturada;
        }

        public Peca InformarPeca(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p;
        }

        public Peca ValidarOpcaoDePromocao(int opcao, Cor cor)
        {
            if (opcao != 1 && opcao != 2 && opcao != 3 && opcao != 4)
            {
                throw new TabuleiroException("Opção Inválida!");
            }
            else
            {
                if (opcao == 1)
                {
                    return new Rainha(tab, cor);
                }
                else if (opcao == 2)
                {
                    return new Bispo(tab, cor);
                }
                else if (opcao == 3)
                {
                    return new Cavalo(tab, cor);
                }
                else if (opcao == 4)
                {
                    return new Torre(tab, cor);
                }
                else
                {
                    throw new TabuleiroException("ERRO!");
                }
            }
        }

        public void RealizarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutarMovimento(origem, destino);

            if (TestarXeque(JogadorAtual))
            {
                desfazerMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.peca(destino);

            // #JogadaEspecial Promocao
            if (p is Peao)
            {
                if ((p.Cor == Cor.Branco && destino.Linha == 1) || (p.Cor == Cor.Preto && destino.Linha == 6))
                {
                    promocao = true;
                }
                if ((p.Cor == Cor.Branco && destino.Linha == 0) || (p.Cor == Cor.Preto && destino.Linha == 7))
                {
                    p = tab.RetirarPeca(destino);
                    Pecas.Remove(p);
                    int opcao = Tela.LerOpcao();
                    Peca Promovida = ValidarOpcaoDePromocao(opcao, p.Cor);
                    tab.colocarPeca(Promovida, destino);
                    Pecas.Add(Promovida);
                    
                }
            }

            if (TestarXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (TestarXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudarJogador();
            }

            // #JogadaEspecial En Passant
            if (p is Peao && destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }

        }

        public void desfazerMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RetirarPeca(destino);
            p.DecrementarQuantMov();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQuantMov();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQuantMov();
                tab.colocarPeca(T, origemT);
            }

            // #jogada especial En Passant
            if (p is Peao && destino.Coluna != origem.Coluna && pecaCapturada is Peao)
            {
                if (p.Cor == Cor.Branco)
                {
                    Peca P = tab.RetirarPeca(destino);
                    Posicao posP;
                    // pela Direita
                    if (destino.Coluna == origem.Coluna + 1)
                    {
                        posP = new Posicao(origem.Linha, origem.Coluna + 1);

                    }
                    // pela esquerda
                    else
                    {
                        posP = new Posicao(origem.Linha, origem.Coluna - 1);

                    }
                    
                    tab.colocarPeca(P, posP);
                }
                else
                {
                    Peca P = tab.RetirarPeca(destino);
                    Posicao posP;
                    // pela Direita
                    if (destino.Coluna == origem.Coluna + 1)
                    {
                        posP = new Posicao(origem.Linha, origem.Coluna + 1);

                    }
                    // pela esquerda
                    else
                    {
                        posP = new Posicao(origem.Linha, origem.Coluna - 1);

                    }
                    
                    tab.colocarPeca(P, posP);
                }
            }

        }

        private void MudarJogador()
        {
            if (JogadorAtual == Cor.Branco)
            {
                JogadorAtual = Cor.Preto;
            }
            else
            {
                JogadorAtual = Cor.Branco;
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao origem)
        {
            if (tab.peca(origem) == null)
            {
                throw new TabuleiroException("Não existe peça na Posicão escolhida!");
            }
            if (tab.peca(origem).Cor != JogadorAtual)
            {
                throw new TabuleiroException("Essa peça não é sua!");
            }
            if (!tab.peca(origem).ValidarMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existe movimentos possíveis para essa peça!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).VerificarMovimentoPossivel(destino))
            {
                throw new TabuleiroException("Essa Peça não pode se mover para essa posição!");
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool TestarXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TestarXequeMate(Cor cor)
        {
            if (!TestarXeque(cor))
            {
                return false;
            }

            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i=0; i < tab.Linhas; i++)
                {
                    for (int j=0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testarXeque = TestarXeque(cor);
                            desfazerMovimento(origem, destino, pecaCapturada);
                            if (!testarXeque)
                            {
                                return false;
                            }

                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            // Peças Brancas
            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branco));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branco));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branco));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branco));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branco));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branco));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branco, this));
            ColocarNovaPeca('d', 1, new Rainha(tab, Cor.Branco));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branco));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branco));

            // Peças Pretas
            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preto, this));
            ColocarNovaPeca('d', 8, new Rainha(tab, Cor.Preto));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preto));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preto));
        }

    }
}
