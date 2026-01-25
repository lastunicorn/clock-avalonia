namespace DustInTheWind.ClockAvalonia.Shapes;

public interface IHand
{
    double Length { get; set; }

    TimeComponent TimeComponent { get; set; }
}
