/*
 * Copyright (C) 2007-2010 Jordi Mas i Hernàndez <jmas@softcatala.org>
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

namespace gbrainy.Games.Calculation
{
	public class CalculationTwoNumbers : Game
	{
		int number_a, number_b;
		int op1, op2, max_operand;
		SubGameTypes type;

		enum SubGameTypes
		{
			Addition,
			Subtraction,
			Length
		};

		public override string Name {
			get {return Catalog.GetString ("Two numbers");}
		}

		public override GameTypes Type {
			get { return GameTypes.MathTrainer;}
		}

		public override string Question {
			get {
				switch (type) {
				case SubGameTypes.Addition:
					return String.Format (Catalog.GetString ("Which two numbers when added are {0} and when multiplied are {1}?"), op1, op2);

				case SubGameTypes.Subtraction:
					return String.Format (Catalog.GetString ("Which two numbers when subtracted are {0} and when multiplied are {1}?"), op1, op2);
				default:
					throw new InvalidOperationException ();
				}
			}
		}

		public override AnswerCheckAttributes CheckAttributes {
			get { return AnswerCheckAttributes.Trim | AnswerCheckAttributes.MatchAll; }
		}

		public override string AnswerCheckExpression {
			get { return "[0-9]+"; }
		}

		public override string AnswerValue {
			get { return String.Format (Catalog.GetString ("{0} and {1}"), number_a, number_b); }
		}

		protected override void Initialize ()
		{
			type = (SubGameTypes) random.Next ((int) SubGameTypes.Length);

			switch (CurrentDifficulty) {
			case Difficulty.Easy:
				max_operand = 8;
				break;
			case Difficulty.Medium:
				max_operand = 10;
				break;
			case Difficulty.Master:
				max_operand = 15;
				break;
			}

			number_a = 5 + random.Next (max_operand);
			number_b = 3 + random.Next (max_operand);

			switch (type) {
			case SubGameTypes.Addition:
				op1 = number_a + number_b;
				break;
			case SubGameTypes.Subtraction:
				if (number_a < number_b) {
					int tmp = number_a;

					number_a = number_b;
					number_b = tmp;
				}
				op1 = number_a - number_b;
				break;
			default:
				throw new InvalidOperationException ();
			}

			op2 = number_a * number_b;
			right_answer = String.Format ("{0} | {1}", number_a, number_b);
		}

		public override void Draw (CairoContextEx gr, int area_width, int area_height, bool rtl)
		{	
			double x = DrawAreaX + 0.1;

			base.Draw (gr, area_width, area_height, rtl);

			gr.SetPangoLargeFontSize ();

			gr.MoveTo (x, DrawAreaY + 0.22);

			switch (type) {
			case SubGameTypes.Addition:
				gr.ShowPangoText (String.Format (Catalog.GetString ("number1 + number2 = {0}"), op1));
				break;
			case SubGameTypes.Subtraction:
				gr.ShowPangoText (String.Format (Catalog.GetString ("number1 - number2 = {0}"), op1));
				break;
			default:
				throw new InvalidOperationException ();
			}
		
			gr.MoveTo (x, DrawAreaY + 0.44);
			gr.ShowPangoText (String.Format (Catalog.GetString ("number1 * number2 = {0}"), op2));
		}

	}
}
