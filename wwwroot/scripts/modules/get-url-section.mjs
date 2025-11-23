export function getUrlSectionByIndex(urlString, position)
{
    try {
        position = position - 1;
        const url = new URL(urlString);
        const pathSegments = url.pathname.split('/').filter(segment => segment.length > 0);
        if (position < 0 || position >= pathSegments.length) {
            throw new Error('Position is out of bounds');
        }
        return pathSegments[position];
    } catch (error) {
        console.error('Invalid URL or error occurred:', error);
        return null;
    }
}