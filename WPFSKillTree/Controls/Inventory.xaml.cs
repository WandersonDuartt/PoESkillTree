﻿using POESKillTree.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POESKillTree.Controls
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : UserControl
    {
        public ItemAttributes ItemAttributes
        {
            get { return (ItemAttributes)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("ItemAttributes", typeof(ItemAttributes), typeof(Inventory), new PropertyMetadata(null));


        public Inventory()
        {
            InitializeComponent();
            Keyboard.AddKeyDownHandler(Application.Current.MainWindow, KeyDown);
        }

        private void ItemVisualizer_DragOver(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffects & DragDropEffects.Link) != 0 && e.Data.GetDataPresent(typeof(ItemVisualizer)))
            {
                var targslot = (ItemSlot)(sender as ItemVisualizer).Tag;
                var itm = (e.Data.GetData(typeof(ItemVisualizer)) as ItemVisualizer).Item;

                if (((int)itm.Class & (int)targslot) != 0 || (itm.Class == ItemClass.TwoHand && targslot == ItemSlot.MainHand))
                {
                    e.Handled = true;
                    e.Effects = DragDropEffects.Link;
                }
                else
                {
                    e.Handled = true;
                    e.Effects = DragDropEffects.None;
                }
            }
        }

        private void ItemVisualizer_Drop(object sender, DragEventArgs e)
        {
            var target = sender as ItemVisualizer;
            if (target != null && (e.AllowedEffects & DragDropEffects.Link) != 0 && e.Data.GetDataPresent(typeof(ItemVisualizer)))
            {
                var vis = e.Data.GetData(typeof(ItemVisualizer)) as ItemVisualizer;
                var targslot = (ItemSlot)(sender as ItemVisualizer).Tag;
                var itm = (e.Data.GetData(typeof(ItemVisualizer)) as ItemVisualizer).Item;

                if (((int)itm.Class & (int)targslot) != 0 || (itm.Class == ItemClass.TwoHand && targslot == ItemSlot.MainHand))
                {
                    e.Handled = true;
                    e.Effects = DragDropEffects.Link;
                    target.Item = vis.Item;
                }
            }
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Point pt = Mouse.GetPosition(this);
                var hit = VisualTreeHelper.HitTest(this, pt);

                if (hit == null)
                    return;

                var hh = hit.VisualHit;

                while (hh != null && !(hh is ItemVisualizer))
                    hh = VisualTreeHelper.GetParent(hh);

                if (hh != null)
                    (hh as ItemVisualizer).Item = null;


            }
        }
    }
}
