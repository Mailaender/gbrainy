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

using gbrainy.Core.Main;
using gbrainy.Core.Libraries;
using gbrainy.Core.Toolkit;

namespace gbrainy.Games.Calculation
{
	public class CalculationCloserFraction : Game
	{
		private double question_num;
		private const int options_cnt = 4;
		private double []options;
		private ArrayListIndicesRandom random_indices;
		private int which;

		public override string Name {
			get {return Catalog.GetString ("Closer fraction");}
		}

		public override Types Type {
			get { return Game.Types.MathTrainer;}
		}

		public override string Question {
			get {return String.Format (
				Catalog.GetString ("Which of the following numbers is closer to {0:##0.###}? Answer {1}, {2}, {3} or {4}."), question_num,
				GetPossibleAnswer (0), GetPossibleAnswer (1), GetPossibleAnswer (2), GetPossibleAnswer (3));}
		}

		public override string Answer {
			get { 
				string answer = base.Answer + " ";
				int ans_idx = random_indices[which];

				answer += String.Format (Catalog.GetString ("The result of the operation {0} / {1} is {2:##0.###}"), 
					options[ans_idx * 2], options[(ans_idx * 2) + 1], question_num);
				return answer;
			}
		}

		public override void Initialize ()
		{	
			options = new double [options_cnt * 2];
			bool duplicated;
			bool done = false;
			int i, ans_idx, basenum, randnum;

			switch (CurrentDifficulty) {
			case Difficulty.Easy:
				basenum = 5;
				randnum = 10;
				break;
			default:
			case Difficulty.Medium:
				basenum = 5;
				randnum = 30;
				break;
			case Difficulty.Master:
				basenum = 9;
				randnum = 60;
				break;
			}

			while (done == false) {
				duplicated = false;
				options[0 + 0] = basenum + random.Next (randnum);
				options[0 + 1] = basenum + random.Next (randnum);
				
				options[2 + 0] = options[0 + 0] + random.Next (2);
				options[2 + 1] = options[0 + 1] + random.Next (2);

				options[(2 * 2) + 0] = options[0 + 0] - random.Next (5);
				options[(2 * 2) + 1] = options[0 + 1] - random.Next (5);
		
				options[(3 * 2) + 0] = options[0 + 0] +  random.Next (5);
				options[(3 * 2) + 1] = options[0 + 1] +  random.Next (5);

				// No repeated answers
				for (int num = 0; num < options_cnt; num++)
				{
					for (int n = 0; n < options_cnt; n++)
					{
						if (n == num) continue;

						if (options [(num * 2) + 0] / options [(num * 2) + 1] ==
							options [(n * 2) + 0] / options [(n * 2) + 1]) {
							duplicated = true;
							break;
						}
					}
				}

				if (duplicated)
					continue;
			
				// No numerator = denominator (1 value)
				if (options [0 + 0] == options [0 + 1]) continue;
				if (options [(1 * 2) + 0] == options [(1 * 2) + 1]) continue;
				if (options [(2 * 2) + 0] == options [(2 * 2) + 1]) continue;
				if (options [(3 * 2) + 0] == options [(3 * 2) + 1]) continue;

				// No < 2
				for (i = 0; i < options_cnt * 2; i++) {
					if (options [i] < 2)
						break;
				}

				if (i < options_cnt * 2)
					continue;
							
				done = true;
			}

			random_indices = new ArrayListIndicesRandom (4);
			random_indices.Initialize ();
		
			which = random.Next (options_cnt);
			ans_idx = random_indices[which];
			question_num = options[ans_idx * 2] / options[(ans_idx * 2) + 1];
			right_answer += GetPossibleAnswer (which);

			// Options
			double x = DrawAreaX + 0.25, y = DrawAreaY + 0.16;
			Container container = new Container (x, y,  1 - (x * 2), 0.6);
			AddWidget (container);
	
			for (i = 0; i < options_cnt; i++)
			{
				DrawableArea drawable_area = new DrawableArea (0.3, 0.1);
				drawable_area.X = x;
				drawable_area.Y = y + i * 0.15;
				container.AddChild (drawable_area);
				drawable_area.Data = i;
				drawable_area.DataEx = GetPossibleAnswer (i);

				drawable_area.DrawEventHandler += delegate (object sender, DrawEventArgs e)
				{
					int n = (int) e.Data;
					int indx = random_indices[n];

					e.Context.SetPangoLargeFontSize ();
					e.Context.MoveTo (0.02, 0.02);
					e.Context.ShowPangoText (String.Format (Catalog.GetString ("{0}) {1}"), GetPossibleAnswer (n) , 
						options [indx * 2] +  " / " + options [(indx  * 2) +1]));
				};
			}
		}
	}
}
