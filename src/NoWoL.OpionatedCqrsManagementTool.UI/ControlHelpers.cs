namespace NoWoL.OpinionatedCqrsManagementTool.UI
{
    public static class ControlHelpers
    {
        public static T? FindParent<T>(IElement element)
            where T : class
        {
            var p = element;

            while (p is not null && p is not T)
            {
                p = p.Parent;
            }

            return p as T;
        }
    }
}