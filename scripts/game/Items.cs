namespace TwelveJam.scripts.game;

public class Items
{
    public float Ferrum, Titan, Silicon, Sulfur;

    public static bool operator >(Items a, Items b)=>
        a.Ferrum > b.Ferrum && a.Sulfur > b.Sulfur && a.Titan > b.Titan &&  a.Silicon > b.Silicon;

    public static bool operator <(Items a, Items b)=>
        a.Ferrum < b.Ferrum && a.Sulfur < b.Sulfur && a.Titan < b.Titan &&  a.Silicon < b.Silicon;
    
    public static bool operator >=(Items a, Items b)=>
        a.Ferrum >= b.Ferrum && a.Sulfur >= b.Sulfur && a.Titan >= b.Titan &&  a.Silicon >= b.Silicon;

    public static bool operator <=(Items a, Items b)=>
        a.Ferrum <= b.Ferrum && a.Sulfur <= b.Sulfur && a.Titan <= b.Titan &&  a.Silicon <= b.Silicon;

    public static Items operator -(Items a, Items b)=> new()
        {
            Ferrum = a.Ferrum - b.Ferrum,
            Silicon = a.Silicon - b.Silicon,
            Titan = a.Titan - b.Titan,
            Sulfur = a.Sulfur - b.Sulfur
        };
    public static Items operator +(Items a, Items b)=> new()
    {
        Ferrum = a.Ferrum + b.Ferrum,
        Silicon = a.Silicon + b.Silicon,
        Titan = a.Titan + b.Titan,
        Sulfur = a.Sulfur + b.Sulfur
    };
}