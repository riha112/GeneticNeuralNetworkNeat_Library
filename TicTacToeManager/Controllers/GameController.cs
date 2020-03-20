using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using DataManager.Models;
using GNNNeatLibrary.Controllers.Net;

namespace TicTacToeManager.Controllers
{
    public class GameController
    {
        private readonly INetNeatController _netNeatController;
        public int[,] Grid { get; set; } = new int[3,3];

        private List<(int y, int x)> _freeSpaces;
        public NetModel PlayerOne { get; set; }
        public NetModel PlayerTwo { get; set; }
        public bool IsPlayersOneMove { get; set; }

        public GameController(INetNeatController netNeatController, NetModel playerOne, NetModel playerTwo = null)
        {
            (PlayerOne, PlayerTwo, IsPlayersOneMove) = (playerOne, playerTwo, true);
            
            _netNeatController = netNeatController;

            _freeSpaces = new List<(int, int)>();
            for (var y = 0; y < 3; y++)
                for (var x = 0; x < 3; x++)
                    _freeSpaces.Add((y, x));
        }

        public bool DoMove(int y, int x)
        {
            if (Grid[x, y] != 0)
                return false;

            Grid[x, y] = IsPlayersOneMove ? 1 : -1;
            IsPlayersOneMove = !IsPlayersOneMove;
            for(var i = 0; i < _freeSpaces.Count; i++)
                if (_freeSpaces[i].x == x && _freeSpaces[i].y == y)
                {
                    _freeSpaces.RemoveAt(i);
                    break;
                }

            DoMove();

            return true;
        }

        public void DoMove()
        {
            if (CheckGrid() != 0) 
                return;

            if (IsPlayersOneMove && PlayerOne != null)
                DoBotMove(PlayerOne);
            else if (!IsPlayersOneMove && PlayerTwo != null)
                DoBotMove(PlayerTwo);
        }

        private void DoBotMove(NetModel bot)
        {
            if(bot == null) return;

            _netNeatController.FeedForward(bot, GridToVector());
            var output = _netNeatController.GetOutputValues(bot);
            var normalizedOutput = Math.Abs(output[0]) * (_freeSpaces.Count - 1);
            var n = (int)Math.Round(normalizedOutput, 0);
            DoMove(_freeSpaces[n].y, _freeSpaces[n].x);
        }

        /// <returns>1 - player one won, -1 - player two one, 2 - draw, 0 - inprogress</returns>
        public int CheckGrid()
        {
            for (var i = 0; i < 3; i++)
            {
                // Horizontal
                if (Grid[i, 0] == Grid[i, 1] && Grid[i, 1] == Grid[i, 2] && Grid[i, 1] != 0)
                    return Grid[i, 0];
                
                // Vertical
                if (Grid[0, i] == Grid[1, i] && Grid[1, i] == Grid[2, i] && Grid[1, i] != 0)
                    return Grid[0, i];
            }

            // Diagonal
            if (Grid[1, 1] != 0)
            {
                if (Grid[0, 0] == Grid[1, 1] && Grid[1, 1] == Grid[2, 2] ||
                    Grid[0, 2] == Grid[1, 1] && Grid[2, 0] == Grid[1, 1])
                    return Grid[1, 1];
            }

            // Draw
            var isDraw = true;
            for (var y = 0; y < 3; y++)
                for (var x = 0; x < 3; x++)
                    if (Grid[y, x] == 0)
                    {
                        isDraw = false;
                        break;
                    }
            return isDraw ? 2 : 0;
        }

        /// <summary>
        /// Converts game grid into input layer values
        /// </summary>
        private double[] GridToVector()
        {
            var output = new double[9];

            for (var y = 0; y < 3; y++)
            {
                for (var x = 0; x < 3; x++)
                {
                    output[y * 3 + x] = Grid[x, y] switch
                    {
                        1 => 0.7,
                        -1 => -0.7,
                        _ => 0.95
                    };
                }
            }

            return output;
        }
    }
}
