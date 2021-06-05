using System;
using tabuleiro;
using tabuleiro.Enums;
using System.Collections.Generic;
using xadrez;

namespace Projeto_Xadrez
{
    class Tela
    {

        public static void ImprimirPartida(PartidaDeXadrez partida)
        {
            ImprimirTabuleiro(partida.tab);

            Console.WriteLine();

            Console.WriteLine("---------------------");
            ImprimirPecasCapturadas(partida);
            Console.WriteLine("---------------------");

            Console.WriteLine();

            if (!partida.Terminada)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("TURNO: " + partida.Turno);
                Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);
                Console.WriteLine("-------------------------");
                if (partida.Xeque)
                {
                    Console.WriteLine(">>>>>>>>>-XEQUE-<<<<<<<<<");
                }
            }
            else
            {
                Console.WriteLine("XEQUE-MATE!");
                Console.WriteLine("Vencedor: " + partida.JogadorAtual);
            }
        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("<PEÇAS CAPTURADAS>");
            Console.Write("Jogador 1: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branco));

            Console.WriteLine();

            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Jogador 2: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preto));
            Console.ForegroundColor = aux;

            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("{");
            foreach(Peca x in conjunto)
            {
                Console.Write(x + " ");
            }
            Console.Write("}");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(tab.Linhas - i + "| ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    ImprimirPeca(tab.peca(new Posicao(i, j)));
                }
                Console.WriteLine();
            }

            Console.WriteLine("   a b c d e f g h");

        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {

            ConsoleColor FundoOriginal = Console.BackgroundColor;
            ConsoleColor FundoCinza = ConsoleColor.DarkGray;
            ConsoleColor FundoVermelho = ConsoleColor.DarkRed;

            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(tab.Linhas - i + "| ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                    {
                        Posicao pos = new Posicao(i, j);
                        if (tab.PecaExiste(pos))
                        {
                            Console.BackgroundColor = FundoVermelho;
                        }
                        else
                        {
                            Console.BackgroundColor = FundoCinza;
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = FundoOriginal;
                    }

                    ImprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = FundoOriginal;

                }
                Console.WriteLine();
            }

            Console.BackgroundColor = FundoOriginal;

            Console.WriteLine("   a b c d e f g h");
        }

        public static PosicaoXadrez LerPosicao()
        {
            string pos = Console.ReadLine();
            char col = pos[0];
            int lin = int.Parse(pos[1] + "");
            return new PosicaoXadrez(col, lin);

        }

        public static int LerOpcao()
        {
            Console.WriteLine();
            Console.WriteLine("**PEÃO PROMOVIDO**");
            Console.WriteLine("Escolha qual peça ele se tornará:");
            Console.WriteLine("(1) Rainha");
            Console.WriteLine("(2) Bispo");
            Console.WriteLine("(3) Cavalo");
            Console.WriteLine("(4) Torre");
            Console.WriteLine();
            Console.Write(">> ");
            int opcao = int.Parse(Console.ReadLine());
            return opcao;
        }

        public static void ImprimirPeca(Peca peca)
        {

            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.Cor == Cor.Branco)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }

        }



    }
}
