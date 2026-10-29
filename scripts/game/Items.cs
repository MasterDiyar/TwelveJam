namespace TwelveJam.scripts.game;

public class Items
{
    public Items(){}
    public Items(float ferrum, float silicon, float titan, float sulfur) {
        Ferrum = ferrum;
        Silicon = silicon;
        Titan = titan;
        Sulfur = sulfur;
    }
    public float Ferrum, Titan, Silicon, Sulfur;

    public static bool operator >(Items a, Items b)=>
        a.Ferrum > b.Ferrum && a.Sulfur > b.Sulfur && a.Titan > b.Titan &&  a.Silicon > b.Silicon;

    public static bool operator <(Items a, Items b)=>
        a.Ferrum < b.Ferrum && a.Sulfur < b.Sulfur && a.Titan < b.Titan &&  a.Silicon < b.Silicon;
    
    public static bool operator >=(Items a, Items b)=>
        a.Ferrum >= b.Ferrum && a.Sulfur >= b.Sulfur && a.Titan >= b.Titan &&  a.Silicon >= b.Silicon;

    public static bool operator <=(Items a, Items b)=>
        a.Ferrum <= b.Ferrum && a.Sulfur <= b.Sulfur && a.Titan <= b.Titan &&  a.Silicon <= b.Silicon;

    public static Items operator -(Items a, Items b)=> new(
             a.Ferrum - b.Ferrum,
             a.Silicon - b.Silicon,
              a.Titan - b.Titan,
              a.Sulfur - b.Sulfur
        );
    public static Items operator +(Items a, Items b)=> new(
        a.Ferrum + b.Ferrum,
        a.Silicon + b.Silicon,
         a.Titan + b.Titan,
        a.Sulfur + b.Sulfur);
    
    public static Items operator *(Items a, float multiplier) => new(
        a.Ferrum * multiplier,
        a.Silicon * multiplier,
        a.Titan * multiplier,
        a.Sulfur * multiplier
    );

    public static Items operator *(float multiplier, Items a) => a * multiplier;

    public bool AnyNegative()
    {
        return Ferrum < 0 || Titan < 0 || Silicon < 0 || Sulfur < 0;
    }
    
    public static Items One() => new Items(1,1,1,1);
}