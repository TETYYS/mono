// Compiler options: -r:test-anon-94-lib.dll

using System;

class Program
{
	public class BaseClass
	{
		public int i;
		public virtual void Print () { Console.WriteLine ("BaseClass.Print"); i = 90; }
	}

	public class Derived : BaseClass
	{
		public override void Print ()
		{
			Action a = () => base.Print ();
			a ();
		}
	}
	
	public class DerivedLibrary : BaseClassLibrary
	{
		public override void Print (int arg)
		{
			Action a = () => base.Print (30);
			a ();
		}
	}

	public static int Main ()
	{
		var d = new Derived ();
		d.Print ();

		if (d.i != 90)
			return 1;

		var d2 = new DerivedLibrary ();
		d2.Print (0);

		if (d2.i != 30)
			return 2;

		return 0;
	}
}
