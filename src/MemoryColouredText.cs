/*
 * Copyright (C) 2007 Jordi Mas i Hernàndez <jmas@softcatala.org>
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
using Cairo;
using Mono.Unix;
using System.Timers;
using Gtk;
using System.Collections;

public class MemoryColouredText : Memory
{
	private ColorPalette palette;
	private int question;
	private string question_colorname;
	private int colors_shown;

	public override string Name {
		get {return Catalog.GetString ("Colored text");}
	}

	public override string Question {
		get {return Catalog.GetString ("Memorize the colors associated to every word"); }
	}

	public override string MemoryQuestion {
		get { 
			return String.Format (Catalog.GetString ("Which was the color of the text that said '{0}'?"), question_colorname);}
	}

	public override void Initialize ()
	{
		switch (CurrentDifficulty) {
		case Difficulty.Easy:
			colors_shown = 3;
			break;
		case Difficulty.Medium:
			colors_shown = 4;
			break;
		case Difficulty.Master:
			colors_shown = 6;
			break;
		}

		palette = new ColorPalette (colors_shown);
		palette.Initialize ();
		
		question = random.Next (palette.Count);
		right_answer = palette.Name (question);
		question_colorname = palette.Name ((ColorPalette.Id) question);
		
		base.Initialize ();
	}
	
	public override void DrawObjectToMemorize (Cairo.Context gr, int area_width, int area_height)
	{
		base.DrawObjectToMemorize (gr, area_width, area_height);
		DrawObject (gr, area_width, area_height);
	}

	private void DrawObject (Cairo.Context gr, int area_width, int area_height)
	{
		palette.Alpha=alpha;

		double x= DrawAreaX + 0.125, y = DrawAreaY + 0.2;

		for (int i = 0; i < palette.Count ; i++)
		{
			gr.Color = palette.Cairo(i);
			gr.MoveTo (x, y);
			gr.ShowText ( palette.Name((ColorPalette.Id)i) );
			gr.Stroke ();
			
			if (i == 2) {
				y += 0.2;
				x = DrawAreaX + 0.125;
			} else {
				x+= 0.25;
			}
		}
	}
}

