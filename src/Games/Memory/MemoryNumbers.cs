/*
 * Copyright (C) 2008 Jordi Mas i Hernàndez <jmas@softcatala.org>
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
using System.Text;
using Mono.Unix;

using gbrainy.Core.Main;
using gbrainy.Core.Libraries;

namespace gbrainy.Games.Memory
{
	public class MemoryNumbers : Core.Main.Memory
	{
		private Challenge current_game;
		private const int num_games = 3;

		public class Challenge
		{
			protected static int [] numbers;

			public Challenge () {}
		
			public static int[] Numbers {
				set { numbers = value;}
				get { return numbers;}
			}

			virtual public string Question {
				get {return string.Empty; }
			}	

			virtual public string Answer {
				get {return string.Empty; }
			}	
		}

		public class ChallengeOdds : Challenge
		{
			public override string Question {
				get {
					return Catalog.GetString ("How many odd numbers were in the previous image? Answer using numbers.");
				}
			}		

			public override string Answer {
				get {
					int odds = 0;
					for (int i = 0; i < numbers.Length; i++) {
						if (numbers[i] % 2 != 0)
							odds++;
					}
					return odds.ToString ();
				}
			}
		}

		public class ChallengeEvens : Challenge
		{
			public override string Question {
				get {
					return Catalog.GetString ("How many even numbers were in the previous image? Answer using numbers.");
				}
			}		

			public override string Answer {
				get {
					int evens = 0;
					for (int i = 0; i < numbers.Length; i++) {
						if (numbers[i] % 2 == 0)
							evens++;
					}
					return evens.ToString ();
				}
			}
		}

		public class ChallengeTwoDigits : Challenge
		{
			public override string Question {
				get {
					return Catalog.GetString ("How many numbers with more than one digit were in the previous image? Answer using numbers.");
				}
			}		

			public override string Answer {
				get {
					int digits = 0;
					for (int i = 0; i < numbers.Length; i++) {
						if (numbers[i] > 9)
							digits++;
					}
					return digits.ToString ();
				}
			}
		}

		public override string Name {
			get {return Catalog.GetString  ("Memorize numbers");}
		}

		public override string MemoryQuestion {
			get { return current_game.Question; }
		}

		protected override void Initialize ()
		{
			base.Initialize ();
			int total;
	
			switch (CurrentDifficulty) {
			case Difficulty.Easy:
				total = 5;
				break;
			case Difficulty.Medium:
			default:
				total = 7;
				break;
			case Difficulty.Master:
				total = 9;
				break;
			}

			int[] nums = new int [total];

			for (int i = 0; i < nums.Length; i++)
				nums[i] = 1 + random.Next (15);

			switch (random.Next (num_games)) {
			case 0: 
				current_game = new ChallengeOdds ();
				break;
			case 1:
				current_game = new ChallengeEvens ();
				break;
			case 2:
				current_game = new ChallengeTwoDigits ();
				break;
			}
		
			Challenge.Numbers = nums;
			right_answer = current_game.Answer;
		}

		public override void DrawObjectToMemorize (CairoContextEx gr, int area_width, int area_height, bool rtl)
		{

			StringBuilder sequence = new StringBuilder (64);

			base.DrawObjectToMemorize (gr, area_width, area_height, rtl);
			gr.SetPangoLargeFontSize ();

			for (int num = 0; num < Challenge.Numbers.Length - 1; num++)
			{
				sequence.Append (Challenge.Numbers [num]);
				sequence.Append (", ");
			}
			sequence.Append (Challenge.Numbers [Challenge.Numbers.Length - 1]);

			gr.DrawTextCentered (0.5, DrawAreaY + 0.3, sequence.ToString ());

		}
	}
}
