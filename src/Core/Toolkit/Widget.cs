/*
 * Copyright (C) 2010 Jordi Mas i Hernàndez <jmas@softcatala.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;
using System.ComponentModel;

using gbrainy.Core.Main;
using gbrainy.Core.Libraries;

namespace gbrainy.Core.Toolkit
{
	public abstract class Widget : IDrawable, IDrawRequest, IMouseEvent
	{
		public delegate void WidgetDrawEventHandler (object sender, DrawEventArgs e);
		public delegate void WidgetSelectedEventHandler (object sender, SeletectedEventArgs e);

		public event EventHandler DrawRequest;
		public event WidgetSelectedEventHandler SelectedEvent;
		ISynchronizeInvoke synchronize;

	    	public Widget (double width, double height)
		{
			Width = width;
			Height = height;
		}

		public bool Sensitive { get; set; }
		public object Data { get; set; }
		public object DataEx { get; set; }

		public double X { get; set; }
		public double Y { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }

		public ISynchronizeInvoke SynchronizingObject { 
			set { synchronize = value; }
			get { return synchronize; }
		}

		protected void OnDrawRequest ()
		{
			if (DrawRequest == null)
				return;

			DrawRequest (this, EventArgs.Empty);
		}

		protected void OnSelected (SeletectedEventArgs e)
		{
			if (SelectedEvent == null)
				return;

			SelectedEvent (this, e);
		}

		public virtual void Draw (CairoContextEx gr, int area_width, int area_height, bool rtl) 
		{
			throw new InvalidOperationException ();
		}

		public virtual void MouseEvent (object obj, MouseEventArgs args)
		{
			throw new InvalidOperationException ();
		}
	}
}
