@ModelType IEnumerable(Of IAS_Handyman.Models.Technician)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.Identification)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.FullName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreationDateTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ModificationDateTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.RegisterStatus)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Identification)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.FullName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreationDateTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ModificationDateTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.RegisterStatus)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.Id }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.Id }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.Id })
        </td>
    </tr>
Next

</table>
