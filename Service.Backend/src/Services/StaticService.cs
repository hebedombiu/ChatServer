using Microsoft.Extensions.Options;
using Service.Backend.Model;
using Service.Backend.Options;

namespace Service.Backend.Services;

public class StaticService {
    private readonly string[] _colors;

    public StaticService(
        IOptions<StaticOptions> options
    ) {
        _colors = options.Value.Colors;
    }

    public StaticData GetStatic() {
        return new StaticData {Colors = _colors};
    }

    public bool HasColor(string color) {
        return _colors.Contains(color);
    }
}