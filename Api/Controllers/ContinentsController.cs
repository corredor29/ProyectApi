using Api.Dtos.Continents;
using Application.Abstractions;
using Domain.ValueObject.Continents;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class ContinentsController : BaseApiControllers  
{
    private readonly IUniOfWork _uow;                          
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ContinentsController(IUniOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContinentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ContinentDto>>> GetAll(CancellationToken ct)
    {
        var continents = await _uow.Continent.GetAllAsync(ct);  // ← singular
        var result = _mapper.Map<IReadOnlyList<ContinentDto>>(continents);
        return Ok(result);
    }

    [HttpGet("paged")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var continents = await _uow.Continent.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Continent.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<ContinentDto>>(continents);

        return Ok(new { page, pageSize, total, items });
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count([FromQuery] string? search = null, CancellationToken ct = default)
    {
        var total = await _uow.Continent.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContinentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContinentDto>> GetById(Guid id, CancellationToken ct)
    {
        var continentId = new ContinentId(id);
        var continent = await _uow.Continent.GetByIdAsync(continentId, ct);

        if (continent is null) return NotFound();

        var result = _mapper.Map<ContinentDto>(continent);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContinentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateContinentRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<Application.UseCase.Continents.CreateContinent>(request);
        var id = await _sender.Send(command, ct);

        var continentId = new ContinentId(id);
        var continent = await _uow.Continent.GetByIdAsync(continentId, ct);

        if (continent is null) return NotFound();

        var result = _mapper.Map<ContinentDto>(continent);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContinentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContinentRequest request, CancellationToken ct)
    {
        var continentId = new ContinentId(id);
        var continent = await _uow.Continent.GetByIdAsync(continentId, ct);

        if (continent is null) return NotFound();

        var command = _mapper.Map<Application.UseCase.Continents.UpdateContinent>(request) with { Id = id };
        await _sender.Send(command, ct);

        var updated = await _uow.Continent.GetByIdAsync(continentId, ct);
        var result = _mapper.Map<ContinentDto>(updated!);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var continentId = new ContinentId(id);
        var continent = await _uow.Continent.GetByIdAsync(continentId, ct);

        if (continent is null) return NotFound();

        await _uow.Continent.RemoveAsync(continent, ct);
        await _uow.SaveChangesAsync(ct);
        return NoContent();
    }
}