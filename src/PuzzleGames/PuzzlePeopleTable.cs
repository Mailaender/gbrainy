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

public class PuzzlePeopleTable : Game
{
	private const double figure_size = 0.15;
	private string ques1, ques2;
	
	private class Circle
	{	
		public double x;
		public double y;

		public Circle (double x, double y) 
		{
			this.x = x;
			this.y = y;
		}
	}

	public override string Name {
		get {return Catalog.GetString ("People at a table");}
	}

	public override string Question {
		get {return String.Format (Catalog.GetString ("A group of people are sitting at round table, evenly spaced out. How many people are there if the {0} person is across from the {1}?"), ques1, ques2);} 
	}

	public override string Answer {
		get { 
			string answer = base.Answer + " ";
			answer += Catalog.GetString ("Subtracting the two positions you find out how many people are seated half way around the table. Doubling this number leaves you with the total amount of people.");
			return answer;
		}
	}

	public override void Initialize ()
	{
		switch (random.Next (3)) {
		case 0:
			ques1 = Catalog.GetString ("5th");
			ques2 = Catalog.GetString ("19th");
			right_answer = "28";
			break;
		case 1:
			ques1 = Catalog.GetString ("4th");
			ques2 = Catalog.GetString ("12th");
			right_answer = "16";
			break;
		case 2:
			ques1 = Catalog.GetString ("9th");
			ques2 = Catalog.GetString ("22nd");
			right_answer = "26";
			break;
		}			
	}

	public override void Draw (CairoContextEx gr, int area_width, int area_height)
	{
		double x = DrawAreaX + 0.22, y = DrawAreaY + 0.2;
		double pos_x = x;
		double pos_y = y;
		Circle[] circles = null;

		base.Draw (gr, area_width, area_height);

		circles =  new Circle [] {
			new Circle (0.01, 0.06),
			new Circle (0.27, 0.06),
			new Circle (0.01, 0.21),
			new Circle (0.27, 0.21),
			new Circle (0.14, 0),
			new Circle (0.14, 0.29)
		};

		// Circle
		gr.Arc (pos_x + figure_size, pos_y + figure_size, figure_size, 0, 2 * Math.PI);
		gr.Stroke ();		

		double point_size = 0.01;
		for (int i = 0; i < circles.Length; i++) {
			gr.Arc (x + point_size + circles[i].x, y + point_size + circles[i].y, point_size, 0, 2 * Math.PI);
			gr.Fill ();
			gr.Stroke ();
		}

		gr.MoveTo (x + circles[2].x + 0.01, y + circles[2].y + 0.01);
		gr.LineTo (x + circles[1].x + 0.01, y + circles[1].y + 0.01);
		gr.Stroke ();
	}

}

