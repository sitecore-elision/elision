Patch files added here are intended for config changes where the default behavior is merely extended. They are patched 
here (instead of the normal Elision folder) so that project-specific config changes can be retained on the default nodes.

For example, project configs can patch site names into the "Sitecore.Publishing.HtmlCacheClearer" like normal, then this
folder is applied to switch the type to use Elision's more efficient cache clearer. If the type was switched earlier,
then project configs would end up creating new Sitecore.Publishing.HtmlCacheClearer entries, instead of simply patching
their sites into the existing one.
