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
using System.Collections;

//
// Returns a list of indexes in random order
//
public class ArrayListIndicesRandom : ArrayList
{
	protected Random random;

	public ArrayListIndicesRandom (int capacity) : base (capacity)
	{
		random = new Random ();
	}

	public void Initialize ()
	{
		ArrayList random_list = new ArrayList (Capacity);
		for (int i = 0; i < Capacity; i++) {
			random_list.Add (i);
		}
		RandomizeFromArray (random_list);
	}

	public void RandomizeFromArray (ArrayList ar)
	{		
		int left = Capacity;
		int index;
		object []array = (object []) ar.ToArray (typeof (object));
		Clear ();

		// Generate a random number that can be as big as the maximum -1
		// Add the random element picked up element in the list
		// The element just randomized gets out of pending list and replaced by the maximum -1 element 
		for (int i = 0; i < Capacity; i++, left--) {
			index = random.Next (left);
			Add (array[index]);
			array[index] = array[left - 1];
		}
	}
}
