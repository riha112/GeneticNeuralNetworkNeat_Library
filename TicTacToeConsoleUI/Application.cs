using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Processors;
using DataManager.Utilities;
using GNNNeatLibrary.Controllers;
using GNNNeatLibrary.Controllers.Net;
using GNNNeatLibrary.Model;
using GNNNeatLibrary.Utilities;
using TicTacToeManager.Controllers;

namespace TicTacToeConsoleUI
{
    public class Application : IApplication
    {
        private ITrainingController _trainingController;
        private INetNeatController _netNeatController;
        private IGnnController _gnnController;
        private GnnModel _currentModel = null;

        public Application(ITrainingController trainingController, INetNeatController netNeatController, IGnnController gnnController)
        {
            _trainingController = trainingController;
            _netNeatController = netNeatController;
            _gnnController = gnnController;
        }

        public void Run()
        {
            Start();
        }

        private void Start()
        {
            var input = -1;
            do
            {
                ClearScreen();
                Console.WriteLine($"Current network: ({_currentModel?.ActiveBatchModel.Id}){_currentModel?.ActiveBatchModel.Name}");
                Console.WriteLine("0. Load network");
                Console.WriteLine("1. New network");
                Console.WriteLine("2. Train network");
                Console.WriteLine("3. Play VS Bot");
                Console.WriteLine("4. End");
                Console.WriteLine("\nEnter action number:");
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch
                {
                    input = -1;
                }

                switch (input)
                {
                    case 0:
                        LoadNetwork();
                        break;
                    case 1:
                        NewNetwork();
                        break;
                    case 2:
                        TrainNetwork();
                        break;
                    case 3:
                        PlayVsBot();
                        break;
                }
            } while (input != 4);

            Console.ReadKey();
        }

        private void PlayVsBot()
        {
            if(_currentModel == null)
                return;
            
            var bestNetworksId = _currentModel.ActiveBatchModel.BestPerformingNetwork;
            var bot = _currentModel.ActiveBatchModel.Networks.Find(n => n.Id == bestNetworksId);
            var game = new GameController(_netNeatController, bot, null);
            do
            {
                ClearScreen();
                game.DoMove();
                DrawGame(game);

                int y = 0, x = 0;
                do
                {
                    var isGoodInput = true;
                    do
                    {
                        try
                        {
                            Console.WriteLine("Enter y (0-2):");
                            y = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter x (0-2):");
                            x = int.Parse(Console.ReadLine());
                            
                            x = Math.Clamp(x, 0, 2);
                            y = Math.Clamp(y, 0, 2);

                            isGoodInput = true;
                        }
                        catch
                        {
                            Console.WriteLine("Incorrect input!");
                            isGoodInput = false;
                        }
                    } while (!isGoodInput);
                } while (!game.DoMove(y, x));


            } while (game.CheckGrid() == 0);
            ClearScreen();
            DrawGame(game);
            Console.WriteLine("Game ended.");
        }

        private void DrawGame(GameController game)
        {
            for (var y = 0; y < 3; y++)
            {
                Console.WriteLine($"{IntToChar(game.Grid[0, y])} | {IntToChar(game.Grid[1, y])} | {IntToChar(game.Grid[2, y])}");
            }
        }

        private char IntToChar(int i) => i switch
        {
            1 => 'X',
            -1 => 'O',
            _ => ' '
        };

        private void TrainNetwork()
        {
            if (_currentModel == null)
            {
                Console.WriteLine("Nothing to train");
                Console.ReadKey();
                return;
            }

            var generationCount = 0;
            var isGoodInput = true;
            do
            {
                try
                {
                    Console.WriteLine("How many generations to train:");
                    generationCount = int.Parse(Console.ReadLine());
                    isGoodInput = true;
                }
                catch
                {
                    isGoodInput = false;
                }
            } while (isGoodInput == false);

            ClearScreen();

            for (var g = 0; g < generationCount; g++)
            {
                Console.WriteLine($"Loading:{g + 1}/{generationCount}");
                _trainingController.TrainGeneration(_currentModel);
            }
        }

        private void NewNetwork()
        {
            Console.WriteLine("Enter name:");
            var name = Console.ReadLine();

            Console.WriteLine("Description:");
            var description = Console.ReadLine();

            _currentModel = _gnnController.New(name, description);
        }

        private void LoadNetwork()
        {
            var id = 0;
            var isGoodInput = true;
            do
            {
                try
                {
                    Console.WriteLine("Id of batch:");
                    id = int.Parse(Console.ReadLine());
                    isGoodInput = true;
                }
                catch
                {
                    isGoodInput = false;
                }
            } while (isGoodInput == false);

            try
            {
                _currentModel = _gnnController.Load(id);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void ClearScreen()
        {
            Console.Clear();
        }
    }
}
