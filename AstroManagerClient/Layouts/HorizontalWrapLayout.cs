using Microsoft.Maui.Layouts;

namespace AstroManagerClient.Layouts;
public class HorizontalWrapLayout : StackLayout
{
    protected override ILayoutManager CreateLayoutManager()
    {
        return new HorizontalWrapLayoutManager(this);
    }
}
