# Umbraco Udi To Id Cache

Rudimentary, potential solution for [Umbraco performance issue](http://issues.umbraco.org/issue/U4-10756) where look-ups via `Umbraco.TypedContent()` via udi are slower than those via the integer Id.

This package will build up a dictionary of a mapping between Udi and Id for all content nodes on start-up via a database query, and keep it up to date following publish events.  

It provides an extension method on `UmbracoHelper` called `TypedContentUsingUdiToIdCache` that accepts a Udi.  Before calling `TypedContent` it first looks up the integer Id from the dictionary and, if found, uses that instead as the parameter.  If not found for some reason, it falls back to using the Udi.