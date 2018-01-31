using BattleshipsServiceLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BattleshipsClient.UserControls
{
    /// <summary>
    /// Interaction logic for BoardUc.xaml
    /// </summary>
    public partial class BoardUc : UserControl
    {
        private List<int> boats = new List<int>() { 5, 4, 3, 3, 2 };

        public Action<int, int> Click { get; set; }

        public Action<int, int> MouseOver { get; set; }

        private Rectangle[,] blocks;

        public BoardUc()
        {
            blocks = new Rectangle[10, 10];
            InitializeComponent();
            for (int i = 1; i < 10; i++)
            {
                Board.Children.Add(new Line() { X1 = 0, X2 = 300, Y1 = i * 30, Y2 = i * 30, Stroke = Brushes.Black, StrokeThickness = 1 });
                Board.Children.Add(new Line() { X1 = i * 30, X2 = i * 30, Y1 = 0, Y2 = 300, Stroke = Brushes.Black, StrokeThickness = 1 });
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = j;
                    int y = i;
                    Rectangle block = new Rectangle();
                    block.Width = 26;
                    block.Height = 26;
                    block.Fill = Brushes.Transparent;
                    block.MouseLeftButtonUp += (s, e) =>
                    {
                        if (Click != null)
                            Click(x, y);
                    };
                    block.MouseEnter += (s, e) =>
                    {
                        if (MouseOver != null)
                            MouseOver(x, y);
                    };
                    Canvas.SetTop(block, y * 30 + 2);
                    Canvas.SetLeft(block, x * 30 + 2);
                    Board.Children.Add(block);
                    blocks[x, y] = block;
                }
            }
        }

        public void SetBlock(int x, int y, BlockState type)
        {
            Dispatcher.Invoke(() =>
            {
                blocks[x, y].Fill = GetBrushByBlockType(type);
            });
        }

        private Brush GetBrushByBlockType(BlockState type)
        {
            switch (type)
            {
                case BlockState.EMPTY: return Brushes.Transparent;
                case BlockState.BOAT: return Brushes.Gray;
                case BlockState.HIT: return Brushes.Red;
                case BlockState.MISS: return Brushes.Yellow;
            }
            return null;
        }

        public void Clear()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    blocks[i, j].Fill = Brushes.Transparent;
        }
    }
}
