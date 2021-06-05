using xadrez;
using System;
using tabuleiro;
using tabuleiro.Exceptions;

namespace Projeto_Xadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada)
                {
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirPartida(partida);

                        // Input da peça que será movida
                        Console.WriteLine();
                        Console.Write("Peça: ");
                        Posicao origem = Tela.LerPosicao().toPosicao();

                        partida.ValidarPosicaoDeOrigem(origem);

                        // Destaque de movimentos
                        bool[,] PosicoesPossiveis = partida.tab.peca(origem).MovimentosPossiveis();

                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.tab, PosicoesPossiveis);


                        // Input de Destino para peça
                        Console.WriteLine();
                        Console.Write("Para: ");
                        Posicao destino = Tela.LerPosicao().toPosicao();

                        partida.ValidarPosicaoDeDestino(origem, destino);


                        //Executa o movimento de acordo com os Inputs
                        partida.RealizarMovimento(origem, destino);

                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                        Console.Write(">>Aperte 'Enter' para continuar<<");
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Tela.ImprimirPartida(partida);

            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

        }
    }
}
