namespace IWKits.Api.Common;

// Namespaces used by this file
using NetTopologySuite.Geometries;
using IWKits.Api.Entities;

// Main content of the file
public sealed record ServiceAreaXNTS
(
	ServiceAreaFull ServiceArea,
	MultiPolygon NtsBoundary
);