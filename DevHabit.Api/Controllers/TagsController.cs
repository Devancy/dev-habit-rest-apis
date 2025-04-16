using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHabit.Api.Database;
using DevHabit.Api.Database.Entities;
using DevHabit.Api.DTOs.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TagsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TagsCollectionDto>> GetTags()
    {
        List<TagDto> tags = await dbContext
            .Tags
            .Select(TagQueries.ProjectToDto())
            .ToListAsync();

        TagsCollectionDto tagsCollection = new(tags);
        return Ok(tagsCollection);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(string id)
    {
        TagDto? tag = await dbContext
            .Tags
            .Select(TagQueries.ProjectToDto())
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tag is null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag([FromBody] CreateTagDto? createTagDto)
    {
        if (createTagDto is null)
        {
            return BadRequest("Tag data is required.");
        }

        Tag tag = createTagDto.ToEntity();
        if (await dbContext.Tags.AnyAsync(t => t.Name == tag.Name))
        {
            return Conflict($"Tag with name '{tag.Name}' already exists.");
        }

        dbContext.Tags.Add(tag);
        await dbContext.SaveChangesAsync();

        TagDto tagDto = tag.ToDto();

        return CreatedAtAction(nameof(GetTag), new { id = tagDto.Id }, tagDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TagDto>> UpdateTag(string id, [FromBody] UpdateTagDto? updateTagDto)
    {
        if (updateTagDto is null)
        {
            return BadRequest("Tag data is required.");
        }

        Tag? tag = await dbContext.Tags.FindAsync(id);
        if (tag is null)
        {
            return NotFound();
        }

        tag.UpdateFromDto(updateTagDto);
        dbContext.Tags.Update(tag);
        await dbContext.SaveChangesAsync();

        TagDto tagDto = tag.ToDto();

        return Ok(tagDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(string id)
    {
        Tag? tag = await dbContext.Tags.FindAsync(id);
        if (tag is null)
        {
            return NotFound();
        }

        dbContext.Tags.Remove(tag);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
