using System;

/**
    Class Fraction represents a fraction in game
    Derived from https://stackoverflow.com/questions/5360196/how-can-you-add-two-fractions
*/
[Serializable]
public struct Fraction
{

    public int numerator;
    public int denominator;

    public Fraction(int numerator, int denominator)
        : this()
    {
        this.numerator = numerator;
        this.denominator = denominator;
    }

    /**
        Simplify a fraction using GCD
    */
    public Fraction Simplify()
    {
        int gcd = GCD();
        return new Fraction(numerator / gcd, denominator / gcd);
    }

    public Fraction InTermsOf(Fraction other)
    {
        return denominator == other.denominator ? this :
            new Fraction(numerator * other.denominator, denominator * other.denominator);
    }

    /**
        Find gcd of numerator and denominator
    */
    public int GCD()
    {
        int a = numerator;
        int b = denominator;
        while (b != 0)
        {
            int t = b;
            b = a % b;
            a = t;
        }
        return a;
    }

    public float ToDecimal()
    {
        return ((float)numerator)/denominator;
    }

    public Fraction Reciprocal()
    {
        return new Fraction(denominator, numerator);
    }

    /**
        Operator overloading for + (adding fractions)
    */
    public static Fraction operator +(Fraction left, Fraction right)
    {
        var left2 = left.InTermsOf(right);
        var right2 = right.InTermsOf(left);

        return new Fraction(left2.numerator + right2.numerator, left2.denominator);
    }

    /**
        Operator overloading for - (subtracting fractions)
    */
    public static Fraction operator -(Fraction left, Fraction right)
    {
        var left2 = left.InTermsOf(right);
        var right2 = right.InTermsOf(left);

        return new Fraction(left2.numerator - right2.numerator, left2.denominator);
    }

    /**
        Operator overloading for * (multiplying fractions)
    */
    public static Fraction operator *(Fraction left, Fraction right)
    {
        return new Fraction(left.numerator * right.numerator, left.denominator * right.denominator);
    }

    /**
        Operator overloading for / (dividing fractions)
    */
    public static Fraction operator /(Fraction left, Fraction right)
    {
        return new Fraction(left.numerator * right.denominator, left.denominator * right.numerator);
    }

    /**
        Operator overloading for creating a new fraction instance given a string represenation e.g. "1/2"
    */
    public static implicit operator Fraction(string value)
    {
        var tokens = value.Split('/');
        int num;
        int den;
        if (tokens.Length == 1 && int.TryParse(tokens[0], out num))
        {
            return new Fraction(num, 1);
        }
        else if (tokens.Length == 2 && int.TryParse(tokens[0], out num) && int.TryParse(tokens[1], out den))
        {
            return new Fraction(num, den);
        }

        return null;
    }

    public override string ToString()
    {
        return string.Format("{0}/{1}", numerator, denominator);
    }
}