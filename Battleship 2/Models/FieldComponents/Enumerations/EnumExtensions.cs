
namespace Battleship_2.Models.FieldComponents.Enumerations
{
    public static class EnumExtensions
    {
        public static OrientationsEnum DirectionToOrientation(this DirectionsEnum directionEnum)
        {
            if (directionEnum == DirectionsEnum.Unknown) return OrientationsEnum.Unknown;

            return directionEnum == DirectionsEnum.Left || directionEnum == DirectionsEnum.Right
                ? OrientationsEnum.Horizontal
                : OrientationsEnum.Vertical;
        }
    }
}
