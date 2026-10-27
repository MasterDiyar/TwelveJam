namespace TwelveJam.scripts.game;

public class Items
{
    public Items(){}
    public Items(float ferrum, float silicon, float titan, float sulfur) {
        Ferrum = ferrum;
        Sulfur = silicon;
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
}