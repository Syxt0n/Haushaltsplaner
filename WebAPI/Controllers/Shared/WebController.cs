using Application.EFCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

public class WebController<T> : ControllerBase
{
	protected readonly ILogger<T> _logger;
	protected HpContext Context;
	protected readonly IConfiguration Config;

	public WebController(ILogger<T> logger, HpContext context, IConfiguration config)
	{
		_logger = logger;
		Context = context;
		Config = config;
	}
}
