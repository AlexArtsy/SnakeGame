﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeGame.Snake
{
    internal class Snake
    {
        #region Поля
        //private readonly State state;
        private readonly GameField gameField;
        public SnakeHead head;
        #endregion

        #region Свойства
        public int Speed { get; set; }
        //  public string HeadDirection { get; set; }
        public SnakeMind Mind { get; set; }
        public List<SnakeMember> Body { get; set; }

        #endregion

        #region Методы

        public void RunSnake()
        {
            Task snakeTask = new Task(() =>
            {
                while (true)
                {
                    Move();
                    var next = this.Mind.ExploreNextCell();
                    if (next.Value.ToString() == "SnakeGame.SnakeFood")
                    {
                        RaiseSnake((SnakeFood)next.Value);
                    }

                    Thread.Sleep(this.head.Speed * 10);
                }
            });

            snakeTask.Start();

            snakeTask.Wait();

        }
        public void Move()
        {
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.Mind.CalculateBodyMovingCoordinates();

            this.Body.ForEach(p =>
            {
                p.Move(this.gameField);
            });

        }

        public void RaiseSnake(SnakeFood food)
        {
            this.Body.Insert(1, new SnakeBodyPart(this.head.Position));
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.head.Move(this.gameField);
            this.head.EatFood(food);
        }
        #endregion
        //private void CreateSnake(int x, int y)
        //{
        //    //var head = new SnakeHead(x, y);
        //    //var body1 = new SnakeBodyPart(GetNextX(head.X, this.HeadDirection), GetNextY(head.Y, this.HeadDirection), this.HeadDirection);
        //    //var body2 = new SnakeBodyPart(GetNextX(body1.X, this.HeadDirection), GetNextY(body1.Y, this.HeadDirection), this.HeadDirection);
        //    //var body3 = new SnakeBodyPart(GetNextX(body2.X, this.HeadDirection), GetNextY(body2.Y, this.HeadDirection), this.HeadDirection);
        //    //var tail = new SnakeTail(GetNextX(body3.X, this.HeadDirection), GetNextY(body3.Y, this.HeadDirection), this.HeadDirection);


        //    //head.Figure = "1";
        //    //body1.Figure = "2";
        //    //body2.Figure = "3";
        //    //body3.Figure = "4";
        //    //tail.Figure = " ";

        //    //this.Body.Add(head);
        //    //this.Body.Add(body1);
        //    //this.Body.Add(body2);
        //    //this.Body.Add(body3);
        //    //this.Body.Add(tail);

        //    //this.head = head;
            
        //}

        #region Конструкторы

        public Snake(int x, int y, GameField gameField, int speed)
        {
            this.head = new SnakeHead(new FieldCoordinates(x, y), State.HeadDirection);
            this.Body = new List<SnakeMember>();
            this.gameField = gameField;
            this.Mind = new SnakeMind(this.Body, this.head, this.gameField);
            this.Speed = speed; 

            this.Mind.CreateSnake();

            this.Speed = speed;
        }

        #endregion
    }
}
